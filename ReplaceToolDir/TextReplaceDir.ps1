Param(
    [Parameter(Mandatory=$True)][string]$inputDir,
    [Parameter(Mandatory=$True)][string]$replacementsFile )

<#
  Replace strings in all files in a directory.
 
  Parameters are -
      .\TextReplaceDir.ps1 inputDirectory replacementsFile
  
  This script takes as input a file containing replacement
  specifications in the form -
      fromString toString
  The script replaces all occurences of each replacement spec
  in all files in the specified input directory. The directory name is
  removed before the replacement is made

  This is NOT a general purpose script. It was written with the
  express intention of amending URLs of Windows HTML help files
  in Windows help studio. 
  
  Version 1    Graham Mumford    14/02/2017
  
  Test Files -

      Input        - .\html\
      Replacements - replacements.txt

      Command      - .\TextReplace.ps1 .\html\ replacements.txt
#>

$error.Clear()
pwd
# set up file names
$currentPath = (Get-Item -Path ".\" -Verbose).FullName
#$pathToFile = $currentPath+"\"+$inputFile
#$pathToFileOut = $currentPath+"\"+$outputFile
$pathToReplacementsFile = $currentPath+"\"+$replacementsFile
$replacementList = New-Object System.Collections.Generic.List[System.String]

# Check that the input directory exists
if (!(Test-Path $inputDir))
{
    Write-Error("Input directory "+$inputDir+" does not exist")
    Exit
}

# Check that the replacements exists
if (!(Test-Path $replacementsFile))
{
    Write-Error("Replacements file "+$replacementsFile+" does not exist")
    Exit
}

# read the replacement file into memory
try
{
   
    $replacementData = get-content $pathToReplacementsFile
}
catch
{
    Write-Error("Failed to read replacements file - error is"+$Error)
    Exit
}

# remove any spurious space chars and drop the directory names from the 
# replacementSpecs
foreach ($replacementSpec in $replacementData)
{
    # remove any spurious space chars
    # each spec may look like -
    # html\from.htm   html\to.htm
    $replacementSpec = $replacementSpec -replace '\s+', ' '

    # split the replacement spec to get to and from strings that look like this -
    # html\from.htm
    try
    {
        $splitString = $replacementSpec.Split(' ')
        $fromString = $splitString[0]
        $toString = $splitString[1]
    }
    catch
    {
        Write-Error("Failed to parse replacement spec :"+$replacementSpec+" error is"+$Error)
        Exit
    }

    # remove the directory from each part of the replacement spec.
    # Change html\from.htm to from.htm and add the from and to parts to an item on
    # a list seperated by :
    # The replacement specs need to be fully qualifed as there are subsets in the files
    # e.g.  defg.htm would match abcdefg.htm as well as the desired defg.htm.
    # The strings are all relative URLS embedded in <A HREF="defg.htm"> tags which
    # works to our advantage.
    try
    {
        $splitString = $fromString.Split('/')
        $fromString = $splitString[1]
        $splitString = $toString.Split('/')
        $toString = $splitString[1]
        $replacementList.Add('"'+$fromString+'":"'+$toString+'"')
    }
    catch
    {
        Write-Error("Failed to remove directory names from "+$replacementSpec+" error is"+$Error)
        Exit
    }

}

# all prep work done. We now need to process each file in imputDir and apply
# all the replacement specs to each one.
foreach ( $inputFile in get-childitem $inputDir )
{
    Write-Output("Processing file "+$inputFile)

    $pathToFile = $inputDir+$inputFile

    # read the input file into memory
    try
    {
        $reader = [System.IO.StreamReader] $pathToFile
        $data = $reader.ReadToEnd()
        $reader.close()
    }
    catch
    {
        Write-Error("Failed to read input file - error is"+$Error)
        Exit
    }
    finally
    {
        if ($reader -ne $null)
        {
            $reader.dispose()
        }
    }

    # apply each replacement to the file data
    foreach ($replacementSpec in $replacementList)
    {
        $splitString = $replacementSpec.Split(':')
        $fromString = $splitString[0]
        $toString = $splitString[1]
        Write-Output("replacing "+$fromString+" to "+$toString)

        # perform the replacement
        $data = $data -replace $fromString, $toString
    }

    # all done!

    # write the modified data to the output file
    try
    {
        $writer = [System.IO.StreamWriter] $pathToFile
        $writer.write($data)
        $writer.close()
    }
    catch
    {
        Write-Error("Failed to write output file - error is"+$Error)
        Exit
    }
    finally
    {
        if ($writer -ne $null)
        {
            $writer.dispose()
        }
    }
}