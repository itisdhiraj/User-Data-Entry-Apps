param([string]$ConfigName="simon", [string]$ManifestName="test")

Push-Location
# Load the current environment variables for this process so that we can reset them at the end
$OriginalEnvironment = get-childitem Env:

try{

	$ConfigFile = $ConfigName
	$ManifestFile = $ManifestName
	
	$ProjectRoot = get-Location
		
	# LOAD XML FILES
	[xml]$Manifest = Get-Content "$ManifestFile";
	[xml]$Variables = Get-Content "$ConfigFile";

	# REPLACE VARIABLES IN MANIFEST/CONFIG FILE (FIRST TIME WITHOUT OVERWRITING VARIABLES TO GET FROM BUILD SERVER)
	foreach($var in Get-ChildItem Env:)
	{
		$varToReplace = '$('+$var.name+')'
		[xml]$Manifest = $Manifest.OuterXml.Replace($varToReplace,$var.Value)
		[xml]$Variables = $Variables.OuterXml.Replace($varToReplace,$var.Value)
	}

	# Replace Nested Variables
	[xml]$SourceXML = $Variables
	foreach($configVariable in $SourceXML.Settings.Variables.ChildNodes)
	{
	   # If this variable is reused in the config file replace as we go through
	   $varToReplace = '$('+$configVariable.name+')'
	   [xml]$Variables = $Variables.OuterXml.Replace($varToReplace,$configVariable.InnerText)
	}

	# LOAD VARIABLES INTO ENVIRONMENT VARIABLES
	foreach($configVariable in $Variables.Settings.Variables.ChildNodes)
	{
	   [Environment]::SetEnvironmentVariable($configVariable.Name, $configVariable.InnerText, "Process")
	}


	# REPLACE VARIABLES IN MANIFEST/CONFIG FILE (SECOND TIME TO ADD ENV VARIABLES THAT DIDN'T EXIST ON FIRST PASS)
	foreach($var in Get-ChildItem Env:)
	{
		$varToReplace = '$('+$var.name+')'
		[xml]$Manifest = $Manifest.OuterXml.Replace($varToReplace,$var.Value)
		[xml]$Variables = $Variables.OuterXml.Replace($varToReplace,$var.Value)
	}
	

	# LOAD KEY VARIABLES
	[string]$DacFX = Join-Path ($Variables.Settings.Variables.DacFXFolder) ("Microsoft.SqlServer.Dac.dll" )
	[string]$TopLevelServerName=$Variables.Settings.Variables.ServerName           # This can be overwritten at step name
	[string]$EnvironmentName=$Variables.Settings.Variables.EnvironmentName


	# LOOP THROUP EACH STEP IN THE CONFIG
	foreach($Step in $Manifest.Settings.Steps.Step)
	{
		
		$StepType = $Step.Type
		$StepName = $Step.Name
		If ($Step.ServerName -eq $null){$ServerName = $TopLevelServerName}

		Write-Host "Performing step $StepName from deployment manifest $ManifestFile. Type $StepType." -BackgroundColor DarkMagenta -ForegroundColor DarkYellow
		
		if ($StepType -like "DACPAC*")
		{
			$DatabaseName = $Step.DatabaseName
			$DACPACLocation = join-path ($ProjectRoot) ($Step.Dacpac)
			
			Add-Type -Path $DacFX
			# TODO
			#If (Test-Path $DACPACLocation){
			#  write-host "DACPAC File found"
			#}Else{
			#  write-host "DACPAC File not found"
			#}
			
			$PROFILE = join-path ($ProjectRoot) ($Step.Profile)

			$dacpac = [Microsoft.SqlServer.Dac.DacPackage]::Load($DACPACLocation)
			
			$ConnectionString = "Server=$ServerName;Database=$DatabaseName;Trusted_Connection=True;"
			$dacServices = New-Object Microsoft.SqlServer.Dac.DacServices $ConnectionString
			
			$dacProfile = [Microsoft.SqlServer.Dac.DacProfile]::Load($PROFILE) 
			$DacDeployOptions = $dacProfile.DeployOptions

			# OVERRIDE DEPLOYMENT OPTIONS WITH .config FILE VALUES
			foreach($prop in $Step.Property)
			{
				$PropertyFullName = $prop.Name
				If ($prop.Value -eq "True")
					{$booleanValue=$True}
				Else
					{$booleanValue=$False}

				$DacDeployOptions."$PropertyFullName" = $booleanValue;
			}

			# OVERRIDE SQLCMD VARIABLES WITH .config FILE VALUES
			foreach($var in $Step.Variable)
			{
				$DacDeployOptions.SqlCommandVariableValues[$var.Name] = $var.Value
			}
	
			# ENABLE LOGGING
			register-objectevent -in $dacServices -eventname Message -source "msg" -action { out-host -in $Event.SourceArgs[1].Message.Message }

			try
			{
				$DACPACParentLocation = (get-item $DACPACLocation).Directory

				if ($StepType -eq "DACPAC_DEPLOY") {
					Write-Host "Deploying database $DatabaseName to server $ServerName" -ForegroundColor Green -BackgroundColor Black
					$dacServices.deploy($dacpac, $DatabaseName, "True", $dacProfile.DeployOptions, $null) | Out-File "$DACPACParentLocation\$DatabaseName.sql"
					Write-Host 'Deployment success'
				}
				if ($StepType -eq "DACPAC_SCRIPT"){
					Write-Host "Scripting database $DatabaseName deployment to server $ServerName" -ForegroundColor Green -BackgroundColor Black
					$dacServices.GenerateDeployScript($dacpac, $DatabaseName, $dacProfile.DeployOptions, $null) | Out-File "$DACPACParentLocation\$DatabaseName.sql"
				}
				if ($StepType -eq "DACPAC_REPORT"){	
					Write-Host "Scripting database $DatabaseName report to server $ServerName" -ForegroundColor Green -BackgroundColor Black
					$dacServices.GenerateDeployReport($dacpac, $DatabaseName, $dacProfile.DeployOptions, $null) | Out-File "$DACPACParentLocation\$DatabaseName.xml"
				}
			}
			catch [Microsoft.SqlServer.Dac.DacServicesException] { 
				throw ('Deployment failed: ''{0}'' Reason: ''{1}''' -f $_.Exception.Message, $_.Exception.InnerException.Message) 
			}  
			finally
			{
				unregister-event -source "msg"
			}
		}

		if ($StepType -eq "EXECUTE_SQL_SCRIPT")
		{
		
			$QueryFile = $Step.FilePath
			$QueryFile =  join-path ($ProjectRoot) ($QueryFile)
			
			$TargetDB = $Step.TargetDatabase
			$vars =  New-Object System.Collections.ArrayList
			foreach($var in $Step.Variable)
			{
				$vars += $var.Name+"="+$var.Value
			}

			write-host "Executing" $QueryFile
			Invoke-Sqlcmd -InputFile $QueryFile -ServerInstance $ServerName -Database $TargetDB -Variable $vars 
			pop-location
		}

		if ($StepType -eq "EXECUTE_SQL_FOLDER")
		{
			 $SQLScriptFolder = join-path ($ProjectRoot) ($Step.FolderPath)
    
			Get-ChildItem $SQLScriptFolder -Filter *.sql | `
			Foreach-Object{
				$QueryFile = $_.FullName
				$TargetDB = $Step.TargetDatabase
				$vars =  New-Object System.Collections.ArrayList
				foreach($var in $Step.Variable)
				{
					$vars += $var.Name+"="+$var.Value
				}
				write-host "Executing" $QueryFile
				Invoke-Sqlcmd -InputFile $QueryFile -ServerInstance $ServerName -Database $TargetDB -Variable $vars
				pop-location
			}
		}

		if ($StepType -eq "SSIS_DEPLOY")
		{
			Add-Type -Path (join-path ($PSScriptRoot) ('References\Microsoft.SqlServer.IntegrationServices.Build.dll'))
			Add-Type -AssemblyName "Microsoft.SqlServer.Management.IntegrationServices, Version=13.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"

			$ssisFolderName = $Step.ssisFolderName
			$ssisProjectName = $Step.ssisProjectName
			$ispac = join-path ($ProjectRoot) ($Step.ispac)

			$sqlInstance = $ServerName
			$sqlConnectionString = "Data Source=$sqlInstance;Initial Catalog=master;Integrated Security=SSPI"
			$sqlConnection = New-Object System.Data.SqlClient.SqlConnection $sqlConnectionString

			#Connect to Integration Service
			$ssisServer = New-Object Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices $sqlConnection

			#Check if catalog is already present, if not create one
			if(!$ssisServer.Catalogs["SSISDB"])
			{
			(New-Object Microsoft.SqlServer.Management.IntegrationServices.Catalog($ssisServer,"SSISDB","P@ssword1")).Create()
			}

			$ssisCatalog = $ssisServer.Catalogs["SSISDB"]

			#Check if Folder is already present, if not create one
			if(!$ssisCatalog.Folders.Item($ssisFolderName))
			{
			(New-Object Microsoft.SqlServer.Management.IntegrationServices.CatalogFolder($ssisCatalog,$ssisFolderName,"Powershell")).Create()
			}

			$ssisFolder = $ssisCatalog.Folders.Item($ssisFolderName)
     
			#Check if project is already deployed or not, if deployed drop it and deploy again
			if($ssisFolder.Projects.Item($ssisProjectName))
			{
			$ssisFolder.Projects.Item($ssisProjectName).Drop()
			}

			$ProperPath = resolve-path ($ispac)

			if(!$ssisFolder.Projects.Item($ssisProjectName))
			{
			$ssisFolder.DeployProject($ssisProjectName,[System.IO.File]::ReadAllBytes($ProperPath));
			}

			#Access deployed project
			$ssisProject = $ssisFolder.Projects.Item($ssisProjectName)
 
			# CONFIGURE PACKAGE PARAMETERS
			$ParamsFilter = '/Settings/SSISPackageParameters[@ProjectName = "'+$ssisProjectName+'"]'
			$ParamsXML = $Variables.selectnodes($ParamsFilter)
			foreach($xParameter in $ParamsXML.Parameter)
			{
				write-host 'Configuring parameters for package: ' $xPackageName.PackageName
				$package = $ssisProject.Packages[$xPackageName.PackageName]
		
				foreach ($xParam in $xPackageName.Param)
				{		
					write-host 'Configuring param: ' $xParam.ParamName '   ' $xParam.Value
					$package.Parameters[$xParam.ParamName].Set([Microsoft.SqlServer.Management.IntegrationServices.ParameterInfo+ParameterValueType]::Literal, $xParam.Value);	
				}
			}

			# CONFIGURE PROJECT PARAMETERS
			$ParamsFilter = '/Settings/SSISProjectParameters[@ProjectName = "'+$ssisProjectName+'"]'
			$ParamsXML = $Variables.selectnodes($ParamsFilter)
			foreach($xParameter in $ParamsXML.Parameter)
			{
				write-host 'Configuring parameters for project: ' $xParameter.Name
				$ssisProject.Parameters[$xParameter.Name].Set([Microsoft.SqlServer.Management.IntegrationServices.ParameterInfo+ParameterValueType]::Literal, $xParameter.Value);  
			}

			$ssisProject.Alter()

		} # Close SSIS Loop


		if ($StepType -eq "SSAS_DEPLOY")
		{
			[string]$Server = $ServerName
			[string]$DBName = $Step.TargetDatabase
			[string]$DSServer = $Step.DataSourceServer
			[string]$DSDBName = $Step.DataSourceDatabaseName
			[string]$ProjectName = $Step.ProjectName
			[string]$BinFolder = join-path ($ProjectRoot) ($Step.BinFolder)

			$exe = $Variables.Settings.Variables.SSASDeploymentTool;

			# Build Paths
			$OutputFile = join-path ($BinFolder) ('DeployOutput.txt')
			$asdatabase = join-path ($BinFolder) ($ProjectName+'.asdatabase')
			$TargetsFile = join-path ($BinFolder) ($ProjectName+'.deploymenttargets')
			$ConfigSettingsFile = join-path ($BinFolder) ($ProjectName+'.ConfigSettings')
			$ConnString = 'Provider=SQLNCLI11.1;Data Source='+$DSServer+';Integrated Security=SSPI;Initial Catalog='+$DSDBName+';Packet Size=32767'

			# Replace Settings Connection String with our Source Database
			[xml]$xmlConfigSettingsFile = Get-Content "$ConfigSettingsFile";
			$xmlConfigSettingsFile.ConfigurationSettings.Database.DataSources.DataSource.ConnectionString = $ConnString
			Set-Content "$ConfigSettingsFile" $xmlConfigSettingsFile.OuterXml;

			# Reset Targets files based on deployment settings
			$Targets = '<DeploymentTarget xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:ddl200="http://schemas.microsoft.com/analysisservices/2010/engine/200" xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" xmlns:ddl300="http://schemas.microsoft.com/analysisservices/2011/engine/300" xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" xmlns:ddl400="http://schemas.microsoft.com/analysisservices/2012/engine/400" xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" xmlns:dwd="http://schemas.microsoft.com/DataWarehouse/Designer/1.0">
			  <Database>'+$DBName+'</Database>
			  <Server>'+$Server+'</Server>
			  <ConnectionString>DataSource='+$DSServer+';Timeout=0</ConnectionString>
			</DeploymentTarget>'

			Set-Content -Path "$TargetsFile" -Value $Targets;

			# Deploy
			& $exe $asdatabase /s:$OutputFile
		} # Close SSAS Loop

	} # Close ForEach Loop

	exit(0)
} # Close try
catch{
	$ErrorMessage = $_.Exception.Message
	Write-Output $ErrorMessage
	Pop-Location
	exit(1)
	}
finally{
	
	$RestoreEnvScript = (join-path ($PSScriptRoot) ("RestoreEnvVariables.ps1"))
	& $RestoreEnvScript $originalEnvironment
	Pop-Location
}


