; Inno Setup script for FindIt.
; Build with installer\build.ps1 (compiles Release, then runs ISCC on this file).
; Output: installer\Output\FindIt-Setup.exe

#define MyAppName "FindIt"
#define MyAppExeName "Findit.exe"
#define MyAppPublisher "Nonprofit Complete"
; Where the license key is saved for all users. Must match Licensing.cs in FindIt.
#define LicenseRegKey "Software\Nonprofit Complete\FindIt"
#define LicenseRegValue "RegistrationKey"
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
Source: "{#BinDir}\NPC.Licensing.dll"; DestDir: "{app}"; Flags: ignoreversion sign

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKA; Subkey: "Software\Classes\.fit"; ValueType: string; ValueName: ""; ValueData: "FindIt.SavedSearch"; Flags: uninsdeletevalue; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch"; ValueType: string; ValueName: ""; ValueData: "FindIt Saved Search"; Flags: uninsdeletekey; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Tasks: fitassoc
Root: HKA; Subkey: "Software\Classes\FindIt.SavedSearch\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Tasks: fitassoc
; The license key validated on the "License key" page, for every user of this PC
Root: HKLM; Subkey: "{#LicenseRegKey}"; ValueType: string; ValueName: "{#LicenseRegValue}"; ValueData: "{code:GetLicenseKey}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Nonprofit Complete"; Flags: uninsdeletekeyifempty
; Remove any "Run as administrator" / compatibility settings the user applied to FindIt
Root: HKLM; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: none; ValueName: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue dontcreatekey
Root: HKCU; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: none; ValueName: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue dontcreatekey

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Remove the install folder even if it existed before Setup ran (only if empty)
Type: dirifempty; Name: "{app}"

[Code]
var
  LicensePage: TInputQueryWizardPage;
  LicenseKey: String;          // the validated key, saved by the [Registry] entry above
  LicenseKeyValid: Boolean;

// Keys are pasted from an email, so ignore any line breaks, spaces or quotes picked up on the way.
function CleanLicenseKey(Key: String): String;
var
  I: Integer;
begin
  Result := '';
  for I := 1 to Length(Key) do
    if (Key[I] > ' ') and (Key[I] <> '"') then
      Result := Result + Key[I];
end;

// Asks FindIt itself ("Findit.exe /checkkey <key>", exit code 0 = valid), so the installer
// uses exactly the same check as the app. Both files are unpacked to a temp folder first.
function IsValidLicenseKey(Key: String): Boolean;
var
  ResultCode: Integer;
begin
  Result := False;
  if Key = '' then
    Exit;
  ExtractTemporaryFile('{#MyAppExeName}');
  ExtractTemporaryFile('NPC.Licensing.dll');
  Result := Exec(ExpandConstant('{tmp}\{#MyAppExeName}'), '/checkkey "' + Key + '"', '',
    SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
end;

function GetLicenseKey(Param: String): String;
begin
  Result := LicenseKey;
end;

procedure InitializeWizard;
begin
  LicensePage := CreateInputQueryPage(wpWelcome,
    'License key', 'Enter your FindIt license key.',
    'Your license key is in the email you received when you purchased FindIt. ' +
    'Copy the whole key and paste it below, then click Next.');
  LicensePage.Add('&License key:', False);

  // Use a key given on the command line (/KEY=...), or the one saved by a previous install.
  LicenseKey := CleanLicenseKey(ExpandConstant('{param:KEY|}'));
  if LicenseKey = '' then
    if RegQueryStringValue(HKLM, '{#LicenseRegKey}', '{#LicenseRegValue}', LicenseKey) then
      LicenseKey := CleanLicenseKey(LicenseKey);
  LicensePage.Values[0] := LicenseKey;
  LicenseKeyValid := IsValidLicenseKey(LicenseKey);
end;

// Upgrades and reinstalls (and /KEY= with a valid key) don't ask again.
function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := (PageID = LicensePage.ID) and LicenseKeyValid;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  Key: String;
begin
  Result := True;
  if CurPageID = LicensePage.ID then
  begin
    Key := CleanLicenseKey(LicensePage.Values[0]);
    LicenseKeyValid := IsValidLicenseKey(Key);
    if LicenseKeyValid then
      LicenseKey := Key
    else
    begin
      MsgBox('That license key isn''t valid. Please copy the whole key from your purchase email and try again.',
        mbError, MB_OK);
      Result := False;
    end;
  end;
end;

// The final gate, which also covers silent installs (they never show the License key page).
function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  if LicenseKeyValid then
    Result := ''
  else
    Result := 'A valid FindIt license key is required. For a silent install, pass it as /KEY=<license key>.';
end;

// Deletes FindIt's registry key, then the "Nonprofit Complete" key above it if nothing else is left in it.
procedure DeleteFindItRegistryKey(RootKey: Integer; UserPath: String);
begin
  RegDeleteKeyIncludingSubkeys(RootKey, UserPath + '{#LicenseRegKey}');
  RegDeleteKeyIfEmpty(RootKey, UserPath + 'Software\Nonprofit Complete');
end;

// FindIt keeps each user's preferences (and a key entered in its Register form) under
// HKEY_CURRENT_USER. Clean up every user who is signed in right now; a user who isn't signed in
// has their registry unloaded, so it can't be reached without loading other people's profiles.
procedure DeleteUserRegistryKeys;
var
  Users: TArrayOfString;
  I: Integer;
begin
  DeleteFindItRegistryKey(HKCU, '');
  if RegGetSubkeyNames(HKEY_USERS, '', Users) then
    for I := 0 to GetArrayLength(Users) - 1 do
      // Skip the "S-1-5-..._Classes" entries, which are file associations rather than users
      if (Pos('_Classes', Users[I]) = 0) then
        DeleteFindItRegistryKey(HKEY_USERS, Users[I] + '\');
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
  begin
    // The machine-wide license key ([Registry] removes it too; this also catches anything added later)
    DeleteFindItRegistryKey(HKLM, '');
    DeleteUserRegistryKeys;
    // Remove the parent (e.g. "Nonprofit Complete") only if nothing else is in it
    RemoveDir(ExtractFileDir(ExpandConstant('{app}')));
  end;
end;