/*
This is a base class with a structure & support methods to allow descendants to
1 - Be serializable (i.e., saved as files)
2 - Easily save their properties to and load properties from the registry
//*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Findit
{
    [Serializable()]
    public class SerializablePreferenceSaver : ISerializable
    {
        public RegistryKey reg;
        public const string c_RegKeyName = @"Software\FindIt";

        public SerializablePreferenceSaver()
        {
            reg = Registry.CurrentUser;
            reg.CreateSubKey(c_RegKeyName);
            reg = Registry.CurrentUser.OpenSubKey(c_RegKeyName, true);
            LoadFromRegistry();
        }

        ~SerializablePreferenceSaver()
        {
            if (reg != null)
            {
                reg.Close();
            }
        }

        public virtual void SaveToRegistry()
        {
            //for descendants to implement
        }

        private void ClearTerms(string startswith)
        {
            foreach (string s in reg.GetValueNames())
            {
                if (s.StartsWith(startswith))
                {
                    reg.DeleteValue(s);
                }
            }
        }

        private void SaveTerms(string regKeyPrefix, ref string[] TermList)
        {
            ClearTerms(regKeyPrefix);
            if (TermList != null)
            {
                for (int i = 0; i < TermList.Length; ++i)
                {
                    reg.SetValue(regKeyPrefix + i.ToString(), TermList[i]);
                }
            }
        }

        private string[] LoadTerms(string regKeyPrefix)
        {
            string[] Result = { "" };
            string[] valuenames = reg.GetValueNames();
            int termcnt = 0;
            for (int i = 0; i < valuenames.Length; ++i)
            {
                if (valuenames[i].StartsWith(regKeyPrefix))
                {
                    Array.Resize(ref Result, ++termcnt);
                    Result[termcnt - 1] = reg.GetValue(valuenames[i], "").ToString();
                }
            }
            return Result;
        }

        public virtual void LoadFromRegistry()
        {
            //to be implemented by descendants
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //to be implemented by descendants
        }

        public virtual void Owner(SerializationInfo info, StreamingContext ctxt)
        {
            //to be implemented by descendants
        }

        public SerializablePreferenceSaver(SerializationInfo info, StreamingContext ctxt)
        {
            //to be implemented by descendants
        }

        private Boolean SafeGetBool(ref SerializationInfo info, string term, Boolean defaultValue)
        {
            try
            {
                return (Boolean)info.GetValue(term, typeof(Boolean));
            }
            catch
            {
                return defaultValue;
            }
        }

        private string SafeGetStr(ref SerializationInfo info, string term, string defaultValue)
        {
            try
            {
                return (string)info.GetValue(term, typeof(string));
            }
            catch
            {
                return defaultValue;
            }
        }

        private string[] SafeGetStrArry(ref SerializationInfo info, string term, string[] defaultValue)
        {
            try
            {
                return (string[])info.GetValue(term, typeof(string[]));
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}
