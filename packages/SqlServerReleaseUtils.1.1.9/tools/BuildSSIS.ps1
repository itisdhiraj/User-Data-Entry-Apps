param([string]$ProjectFile)



	# STATICS
	$MsBuild = $env:systemroot + "\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";

	# DEPENDANCIES
	Add-Type -Path (join-path ($PSScriptRoot) ('References\Microsoft.SqlServer.IntegrationServices.Build.dll'))

	[Environment]::SetEnvironmentVariable("Configuration", "Development", "Process")
	[Environment]::SetEnvironmentVariable("SSISProjFile", $ProjectFile, "Process")

	$dtproj = (join-path ($PSScriptRoot) ("ssis.dtproj"))

	# BUILD SSIS PROJECT
	write-host "Build SSIS Project"  -foregroundcolor green -backgroundcolor black 
		& $msbuild $dtproj /t:SSISBuild

	if ($LastExitCode -eq 1)
	{
		throw "SSIS Build Failed"
	}

	Remove-Item Env:\Configuration
	Remove-Item Env:\SSISProjFile


