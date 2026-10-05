; Inno Setup script for FindIt.
; Build with installer\build.ps1 (compiles Release, then runs ISCC on this file).
; Output: installer\Output\FindIt-Setup.exe

#define MyAppName "FindIt"
#define MyAppExeName "Findit.exe"
#define MyAppPublisher "Nonprofit Complete"
#define BinDir "..\Findit\bin\Release"
; Version comes from AssemblyFileVersion in Properties\AssemblyInfo.cs
#define MyAppVersion GetVersionNumbersString(BinDir + "\" + MyAppExeName)

[Setup]
; AppId identifies this app for upgrades/uninstall - never change it once shipped.
AppId={{6D004363-CE20-458B-911D-B8E8DE14573C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
VersionInfoVersion={#MyAppVersion}
; {autopf} = C:\Program Files on 64-bit Windows (the app is AnyCPU, so it runs as 64-bit there)
DefaultDirName={autopf}\Nonprofit Complete\Findit
; Always show the "Select Destination Location" page, even when upgrading an existing install
DisableDirPage=no
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\{#MyAppExeName}
SetupIconFile=..\Findit\Binoculars.ico
OutputDir=Output
OutputBaseFilename=FindIt-Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
; Install in 64-bit mode on x64/ARM64 so {autopf} is "Program Files", not "Program Files (x86)"
ArchitecturesInstallIn64BitMode=x64compatible
ChangesAssociations=yes
; .NET Framework 4.8 ships with Windows 10 1903+ and Windows 11
MinVersion=10.0
SignTool=artifact
SignedUninstaller=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "fitassoc"; Description: "Open saved searches (.fit files) with {#MyAppName}"; GroupDescription: "File associations:"

[Files]
Source: "{#BinDir}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion sign
; Only declares the .NET 4.8 runtime; if 4.8 is missing, Windows prompts to install it instead of failing oddly
Source: "{#BinDir}\{#MyAppExeName}.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinDir}\EPocalipse.IFilter.dll"; DestDir: "{app}"; Flags: ignoreversion sign

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKA; Subkey: "Software\Classes\.fit"; ValueType: string; ValueName: ""; ValueData: "FindIt.SavedSearch"; Flags: uninsdeletevalue; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch"; ValueType: string; ValueName: ""; ValueData: "FindIt Saved Search"; Flags: uninsdeletekey; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Tasks: fitassoc
; Remove any "Run as administrator" / compatibility settings the user applied to FindIt
Root: HKLM; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: none; ValueName: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue dontcreatekey
Root: HKCU; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: none; ValueName: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue dontcreatekey

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Remove the install folder even if it existed before Setup ran (only if empty)
Type: dirifempty; Name: "{app}"

[Code]
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
    // Remove the parent (e.g. "Nonprofit Complete") only if nothing else is in it
    RemoveDir(ExtractFileDir(ExpandConstant('{app}')));
end;