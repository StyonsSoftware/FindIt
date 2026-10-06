using System;
using Microsoft.Win32;

namespace Findit
{
  /// <summary>
  /// License key checks. A key can come from two places:
  /// - the installer, which saves it machine-wide (HKLM) so every user of the PC is licensed
  /// - the Register form, which saves it for the current user (HKCU), for installs without a saved key
  /// </summary>
  internal static class Licensing
  {
    public const string ProductName = "FindIt";
    // Must match the [Registry] entry in installer\FindIt.iss
    private const string c_MachineKeyName = @"Software\Nonprofit Complete\FindIt";
    private const string c_MachineValueName = "RegistrationKey";

    public static bool IsValidKey(string licenseKey)
    {
      return new NPC.Licensing.LicenseKeyHelper().ProductFromKey(licenseKey) == ProductName;
    }

    public static bool IsProductRegistered()
    {
      if (IsValidKey(MachineLicenseKey()))
      {
        return true;
      }
      using (RegistrationPreferences rp = new RegistrationPreferences())
      {
        return IsValidKey(rp.RegistrationKey);
      }
    }

    /// <summary>
    /// The key saved by the installer. The installer runs in 64-bit mode, so read the 64-bit
    /// registry view explicitly (on 32-bit Windows this falls back to the only view there is).
    /// </summary>
    private static string MachineLicenseKey()
    {
      //note that the license key is stored at the machine_key level, whereas other keys are in current_user
      try
      {
        using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
        using (RegistryKey key = hklm.OpenSubKey(c_MachineKeyName))
        {
          return key?.GetValue(c_MachineValueName) as string ?? "";
        }
      }
      catch (System.Security.SecurityException)
      {
        return "";
      }
    }

    /// <summary>
    /// Handles "Findit.exe /checkkey &lt;key&gt;", which the installer runs to validate the key the
    /// user typed. Returns the process exit code: 0 if the key is valid, 1 if not.
    /// </summary>
    public static int CheckKeyCommand(string[] args)
    {
      string key = args.Length > 1 ? args[1] : "";
      return IsValidKey(key) ? 0 : 1;
    }
  }
}
