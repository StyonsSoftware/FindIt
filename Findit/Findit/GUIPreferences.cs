/*
This class represents the various options that we can define for the UI.
This is distinct from the search parameters themselves.

For example, the list of "recent searches" is an artifact of the application, not any
particular search.
 
It is mostly a collection of public properties, to hold folder names and boolean values.

It also includes functionality to save the preferences *to* the registry, and
to load them back *from* the registry.
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
    public class GUIPreferences : SerializablePreferenceSaver
    {
        private string[] m_RecentSavedSearches = { "" };
        private string[] m_RecentSearchFolders = { "" };
        private string m_CustomEditorExe = "";
        private Boolean m_RunSearchesAfterLoad = false;
        private Boolean m_BlinkOnFirst = false;
        private Boolean m_BlinkOnEvery = false;
        private Boolean m_BlinkOnFinish = false;
        private int m_SearchThreadCount = 1;

        private const string c_RecentSavedSearches = "RecentSavedSearches";
        private const string c_RecentSearchFolders = "RecentSearchFolders";
        private const string c_CustomEditorExe = "CustomEditorExe";
        private const string c_RunSearchesAfterLoad = "RunSearchesAfterLoad";
        private const string c_SearchThreadCount = "SearchThreadCount";
        private const string c_BlinkOnFirst = "BlinkOnFirst";
        private const string c_BlinkOnEvery = "BlinkOnEvery";
        private const string c_BlinkOnFinish = "BlinkOnFinish";

        public string[] RecentSavedSearches
        {
            get
            {
                return m_RecentSavedSearches;
            }
            set
            {
                m_RecentSavedSearches = value;
            }
        }

        public string[] RecentSearchFolders
        {
            get
            {
                return m_RecentSearchFolders;
            }
            set
            {
                m_RecentSearchFolders = value;
            }
        }

        public string CustomEditorExe
        {
            get
            {
                return m_CustomEditorExe;
            }
            set
            {
                m_CustomEditorExe = value;
            }
        }

        public int SearchThreadCount
        {
            get
            {
                return m_SearchThreadCount;
            }
            set
            {
                m_SearchThreadCount = value;
            }
        }

        public Boolean RunSearchesAfterLoad
        {
            get
            {
                return m_RunSearchesAfterLoad;
            }
            set
            {
                m_RunSearchesAfterLoad = value;
            }
        }

        public GUIPreferences()
        {
            reg = Registry.CurrentUser;
            reg.CreateSubKey(c_RegKeyName);
            reg = Registry.CurrentUser.OpenSubKey(c_RegKeyName, true);
            LoadFromRegistry();
        }

        ~GUIPreferences()
        {
            if (reg != null)
            {
                reg.Close();
            }
        }

        public override void SaveToRegistry()
        {
            SaveTerms(c_RecentSavedSearches, ref m_RecentSavedSearches);
            SaveTerms(c_RecentSearchFolders, ref m_RecentSearchFolders);
            reg.SetValue(c_CustomEditorExe, CustomEditorExe);
            reg.SetValue(c_RunSearchesAfterLoad, RunSearchesAfterLoad.ToString());
            reg.SetValue(c_SearchThreadCount, SearchThreadCount.ToString());
            reg.SetValue(c_BlinkOnEvery, BlinkOnEvery.ToString());
            reg.SetValue(c_BlinkOnFinish, BlinkOnFinish.ToString());
            reg.SetValue(c_BlinkOnFirst, BlinkOnFirst.ToString());
        }

        public static string DefaultCustomEditor()
        {
            return @"C:\windows\notepad.exe";
        }

        public static Boolean DefaultRunSearchesAfterLoad()
        {
            return false;
        }

        public static Boolean DefaultBlinkOnFirst()
        {                            
            return false;            
        }                            
                                     
        public static Boolean DefaultBlinkOnEvery()
        {                            
            return false;            
        }                            
                                     
        public static Boolean DefaultBlinkOnFinish()
        {
            return false;
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

        public override void LoadFromRegistry()
        {
            RecentSavedSearches = LoadTerms(c_RecentSavedSearches);
            RecentSearchFolders = LoadTerms(c_RecentSearchFolders);
            CustomEditorExe = reg.GetValue(c_CustomEditorExe, DefaultCustomEditor()).ToString();
            RunSearchesAfterLoad = (reg.GetValue(c_RunSearchesAfterLoad, DefaultRunSearchesAfterLoad().ToString()).ToString() == true.ToString());
            string strThreadCount = reg.GetValue(c_SearchThreadCount, Globals.RecommendedSearchThreadCount.ToString()).ToString();
            int iSearchThreadCount = 0;
            int.TryParse(strThreadCount, out iSearchThreadCount);
            SearchThreadCount = iSearchThreadCount;
            BlinkOnEvery = (reg.GetValue(c_BlinkOnEvery, DefaultBlinkOnEvery().ToString()).ToString() == true.ToString());
            BlinkOnFinish = (reg.GetValue(c_BlinkOnFinish, DefaultBlinkOnFinish().ToString()).ToString() == true.ToString());
            BlinkOnFirst = (reg.GetValue(c_BlinkOnFirst, DefaultBlinkOnFirst().ToString()).ToString() == true.ToString());

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("RecentSavedSearches",this.RecentSavedSearches);
        }

        public override void Owner(SerializationInfo info, StreamingContext ctxt)
        {
            this.RecentSavedSearches = (string[])info.GetValue("RecentSavedSearches",typeof(string[]));
        }

        public GUIPreferences(SerializationInfo info, StreamingContext ctxt)
        {
            this.RecentSavedSearches = SafeGetStrArry(ref info, "RecentSavedSearches", Util.EmptyStringArray());
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
        public Boolean BlinkOnFirst
        {
            get
            {
                return m_BlinkOnFirst;
            }
            set
            {
                m_BlinkOnFirst = value;
            }
        }
        public Boolean BlinkOnEvery
        {
            get
            {
                return m_BlinkOnEvery;
            }
            set
            {
                m_BlinkOnEvery = value;
            }
        }
        public Boolean BlinkOnFinish
        {
            get
            {
                return m_BlinkOnFinish;
            }
            set
            {
                m_BlinkOnFinish = value;
            }
        }

    }
}
