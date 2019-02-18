 param(
 [parameter(Mandatory=$TRUE)]
 [System.Collections.DictionaryEntry[]] $oldEnv
 )
 
 # Returns the current environment.
function Get-Environment {
 get-childitem Env:
}
 
 
 # Removes any added variables.
 compare-object $oldEnv $(Get-Environment) -property Key -passthru |
 where-object { $_.SideIndicator -eq "=>" } |
 foreach-object { remove-item Env:$($_.Name) }
 # Reverts any changed variables to original values.
 compare-object $oldEnv $(Get-Environment) -property Value -passthru |
 where-object { $_.SideIndicator -eq "<=" } |
 foreach-object { set-item Env:$($_.Name) $_.Value }
