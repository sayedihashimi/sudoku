[cmdletbinding()]
param()


function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$scriptDir = ((Get-ScriptDirectory) + "\")
$outputPath = (Join-Path $scriptDir 'OutputRoot');

[System.IO.FileInfo]$slnFile = (Join-Path $scriptDir Sudoku.sln)

task default -dependsOn build

task init{
    requires -nameorurl psbuild -version '1.1.5-beta' -noprefix -condition (-not (Get-Command -Module psbuild -Name Invoke-MSBuild -ErrorAction SilentlyContinue) )
    if(-not(Test-Path $outputPath)){
        New-Item -ItemType Directory -Path $outputPath
    }
}

task build{
    Import-NugetPowershell
    Execute-CommandString 'dnvm list'
    set-location $slnFile.Directory.FullName
    Execute-CommandString 'dnu restore'
    Invoke-MSBuild -projectsToBuild ($slnFile.FullName) -configuration Release -visualStudioVersion 14.0 -outputPath $outputPath
}
