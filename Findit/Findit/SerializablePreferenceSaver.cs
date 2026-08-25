/*
This is a base class with a structure & support methods to allow descendants to
1 - Be serializable (i.e., saved as files)
2 - Easily save their properties to and load properties from the registry
//*/
using System;
using Microsoft.Win32;
using System.Runtime.Serialization;

namespace Findit
{
    [Serializable()]
    public class SerializablePreferenceSaver : ISerializable, IDisposable
    {
        public RegistryKey reg;
        public const string c_RegKeyName = @"Software\FindIt";

        public SerializablePreferenceSaver()
        {
            reg = Registry.CurrentUser.CreateSubKey(c_RegKeyName);
            LoadFromRegistry();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (reg != null)
            {
                reg.Close();
                reg = null;
            }
        }

        ~SerializablePreferenceSaver()
        {
            Dispose(false);
        }

        public virtual void SaveToRegistry()
        {
            //for descendants to implement
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
    }
}
