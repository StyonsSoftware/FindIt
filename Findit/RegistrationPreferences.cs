using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
  internal class RegistrationPreferences: SerializablePreferenceSaver
  {
    private const string c_RegistrationKey = "RegistrationKey";
    public string RegistrationKey;

    public RegistrationPreferences()
    {
      //calls LoadFromRegistry
    }

    public override void LoadFromRegistry()
    {
      RegistrationKey = "";
      object k = reg.GetValue(c_RegistrationKey);
      if (k != null)
      {
        RegistrationKey = k.ToString();
      }
    }

    public override void SaveToRegistry()
    {
      reg.SetValue(c_RegistrationKey, RegistrationKey);
    }
  }
}
