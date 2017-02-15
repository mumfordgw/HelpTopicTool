<#
    TextReplaceDirTest1.ps1

    An automated test for the TextReplaceDir.ps1 script

    Version 1    Graham Mumford    13/02/2017

#>

# command to run - 
# .\TextReplaceDirTest1.ps1

# copies dir html.orig to dir html
# runs command .\TextReplaceDir html replace.txt
# compares files html\VCON00001.htm with html.master\VCON00001_MASTER.htm and
# html\VCON00002.htm html.master\VCON00002_MASTER.htm

pwd
$error.Clear()

# set up file names
$currentPath = (Get-Item -Path ".\" -Verbose).FullName
$freshInputDir = ".\html.orig"
$inputDir = ".\html\"
$masterFileDir = ".\html.master\"
$replacementsFile = ".\replacements.txt"

# create fresh set of input files

# remove any existing html directory
if (Test-Path $inputDir)
{

    # Remove the folder
    try
    {
        Remove-Item $inputDir -Recurse
    }
    catch
    {
        Write-Output("errors occurred removing old input directory - may not already exist "+$Error)
    }
}

# copy in the fresh input files
try
{
        $copyFrom = $freshInputDir+"\*"
        mkdir $inputDir
        Copy-Item $copyFrom $inputDir -Recurse
}
catch
{
    Write-Error("error occurred making fresh input directory -"+$Error)
    Exit
}
ls
ls html


# run the script
try
{
    .\TextReplaceDir.ps1 $inputDir $replacementsFile
}
catch
{
    Write-Error("Failed to run script - error is"+$Error)
    Exit
}
# Compare the output files
$file1 = $inputDir+"VCON00001.htm"
$file1Master = $masterFileDir+"VCON00001_MASTER.htm"
try
{
    $diffs = Compare-Object $(get-content $file1) $(get-content $file1Master)
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
