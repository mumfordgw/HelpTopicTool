Param(
    [Parameter(Mandatory=$True)][string]$inputFile,
    [Parameter(Mandatory=$True)][string]$outputFile,
    [Parameter(Mandatory=$True)][string]$replacementsFile )

<#
  Replace strings in a file.
 
  Parameters are -
      .\TextReplace.ps1 inputFile outputFile replacementsFile
  
   Version 1    Graham Mumford    30/01/2017
  
   Test Files -

       Input        - hsc599_hhk.htm
       Output       - hsc599_mod.htm
       Replacements - replacements.txt

       Command      - .\TextReplace.ps1 hsc599_hhk.htm hsc599_mod.htm replacements.txt
#>

pwd
# set up file names
$currentPath = (Get-Item -Path ".\" -Verbose).FullName
$pathToFile = $currentPath+"\"+$inputFile
$pathToFileOut = $currentPath+"\"+$outputFile
$pathToReplacementsFile = $currentPath+"\"+$replacementsFile

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
}
finally
{
   if ($reader -ne $null)
   {
       $reader.dispose()
   }
}

# read the replacement file into memory
try
{
   
   # The lines below that are commented out read the whole file
   # into a single array element which isn't too helpful in this
   # scenario
   #$reader = [System.IO.StreamReader] $pathToReplacementsFile
   #$replacementData = $reader.ReadToEnd()
   #$reader.close()

   $replacementData = get-content $pathToReplacementsFile
}
catch
{
    Write-Error("Failed to read replacements file - error is"+$Error)
}
#finally
#{
#   if ($reader -ne $null)
#   {
#       $reader.dispose()
#   }
#}

# perform the replacements

# BUG!!! reads whole file into single element in the array!!

foreach ($replacementSpec in $replacementData)
{
    # remove any spurious space chars
    $replacementSpec = $replacementSpec -replace '\s+', ' '

    # split the replacement spec
    try
    {
        $splitString = $replacementSpec.Split(' ')
        $fromString = $splitString[0]
        $toString = $splitString[1]
    }
    catch
    {
        Write-Error("Failed to parse replacement spec :"+$replacementSpec+" error is"+$Error)
    }

    # perform the replacement
    $data = $data -replace $fromString, $toString
}

# write the modified data to the output file
try
{
   $writer = [System.IO.StreamWriter] $pathToFileOut
   $writer.write($data)
   $writer.close()
}
catch
{
    Write-Error("Failed to write output file - error is"+$Error)
}
finally
{
   if ($writer -ne $null)
   {
       $writer.dispose()
   }
}
