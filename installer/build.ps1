# Builds FindIt, Signs findit, Builds the installer, then signs that as well.
# This is executed as a post-build event from the IDE.
# When executed in that way, the "SkipBuild" parameter is specified, because the IDE has done the build in that scenario.
# If you execute this directly from PowerShell, then it will invoke MSBuild to do the build from here.
# Usage: powershell -ExecutionPolicy Bypass -File installer\build.ps1 [-SkipBuild]
# If this fails with credential issues, try running "az login" to re-authenticate with azure.
param([switch]$SkipBuild)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (-not $msbuild) { throw 'MSBuild not found. Install Visual Studio or the Build Tools.' }

$iscc = foreach ($ver in 7, 6) {
  # ProgramW6432 is the real "Program Files" even when this runs as 32-bit PowerShell
  # (e.g. launched from 32-bit MSBuild as a post-build event)
  "$env:ProgramW6432\Inno Setup $ver\ISCC.exe"
  "$env:ProgramFiles\Inno Setup $ver\ISCC.exe"
  "${env:ProgramFiles(x86)}\Inno Setup $ver\ISCC.exe"
  "$env:LOCALAPPDATA\Programs\Inno Setup $ver\ISCC.exe"
}
$iscc = $iscc | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $iscc) { throw 'Inno Setup not found. Install it with: winget install JRSoftware.InnoSetup' }

# Build the application itself, unless asked not to.
if (-not $SkipBuild) {
  & $msbuild "$root\Findit.sln" /p:Configuration=Release /t:Rebuild /v:minimal /nologo /p:PostBuildEvent=
  if ($LASTEXITCODE -ne 0) { throw "MSBuild failed ($LASTEXITCODE)" }
}

# Build the installer.  The installer's config file (Findit.iss) takes care of signing the exe.
# Search for the newest installed version of the signing tool.
#$signtool = "${env:ProgramFiles(x86)}\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe"
$signtool = Get-ChildItem "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\signtool.exe" |
  Sort-Object FullName -Descending | Select-Object -First 1 -ExpandProperty FullName
if (-not $signtool) { throw 'signtool.exe not found. Install the Windows SDK.' }
$dlib = "$env:LOCALAPPDATA\Microsoft\MicrosoftArtifactSigningClientTools\Azure.CodeSigning.Dlib.dll"
if (-not (Test-Path $dlib)) { throw 'Artifact Signing dlib not found. Install it with: winget install Microsoft.Azure.ArtifactSigningClientTools' }

$signCmd  = "`$q$signtool`$q sign /fd SHA256 /tr http://timestamp.acs.microsoft.com /td SHA256 /dlib `$q$dlib`$q /dmdf `$q$PSScriptRoot\signing.json`$q `$f"

& $iscc "/Sartifact=$signCmd" "$PSScriptRoot\FindIt.iss"

if ($LASTEXITCODE -ne 0) { throw "ISCC failed ($LASTEXITCODE)" }

Get-ChildItem "$PSScriptRoot\Output\*.exe" | Select-Object Name, Length, LastWriteTime

Copy-Item "$PSScriptRoot\Output\*.exe" "C:\Users\josep\source\repos\SSCWebSite\site\nonprofit-complete.com\downloads\" -force