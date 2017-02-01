<#
    TextReplaceTest1.ps1

    An automated test for the TextReplace.ps1 script

    Version 1    Graham Mumford    01/02/2017

#>

# command to run - 
# .\TextReplace.ps1 hsc599_hhk.htm hsc599_mod.htm replacements.txt

# set up file names
$currentPath = (Get-Item -Path ".\" -Verbose).FullName
$inputFile = "hsc599_hhk.htm"
$outputFile = "hsc599_mod.htm"
$replacementsFile = "replacements.txt"
$pathToOutputFile = $currentPath+"\"+$outputFile
$pathToMasterOutputFile = $currentPath+"\"+"hsc599_mod_CHECKED.htm"

# run the script
try
{
    .\TextReplace.ps1 $inputFile $outputFile $replacementsFile
}
catch
{
    Write-Error("Failed to run script - error is"+$Error)
}
try
{
    $diffs = Compare-Object $(get-content $pathToOutputFile) $(get-content $pathToMasterOutputFile)
}
catch
{
    Write-Error("Comparison of the output file failed - error is"+$Error)
}
if ($diffs.count -ne 0)
{
    Write-Host "ERROR - Comparison failed" -ForegroundColor Red
    $diffs
}
else
{
    Write-Host "Test successful" -ForegroundColor Green
}