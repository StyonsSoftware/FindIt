# Compiles the Inno Setup installer.
# Usage: powershell -ExecutionPolicy Bypass -File installer\build.ps1
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

#un-comment this if you want this script to build FindIt.  we are running this as a post-build event from the IDE, so it's commented out.
#& $msbuild "$root\Findit.sln" /p:Configuration=Release /t:Rebuild /v:minimal /nologo
#if ($LASTEXITCODE -ne 0) { throw "MSBuild failed ($LASTEXITCODE)" }

& $iscc "$PSScriptRoot\FindIt.iss"
if ($LASTEXITCODE -ne 0) { throw "ISCC failed ($LASTEXITCODE)" }

Get-ChildItem "$PSScriptRoot\Output\*.exe" | Select-Object Name, Length, LastWriteTime
