/*
This class represents the various options that we can define a search.
It is mostly a big collection of public properties, to hold folder names and boolean values.

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
    public class SearchParameters : SerializablePreferenceSaver
    {
        private string m_SearchFolder = "";
        private string m_FileTypeFilter = "";
        private string m_FileExcludeFilter = "";        
        private bool m_IncludeLineNosInOutput = false;
        private bool m_IncludePerfStats = false;
        private bool m_CaseSensitive = false;
        private bool m_SearchSubfolders = true;
        private bool m_OnlySearchFileNames = false;
        private bool m_IncludeOffice = false;
        private string[] m_SearchTerms = { "" };
        private string[] m_ExcludeTerms = { "" };

        private const string c_SearchFolder = "SearchFolder";
        private const string c_FileTypeFilter = "FileTypeFilter";
        private const string c_FileExcludeFilter = "FileExcludeFilter";        
        private const string c_IncludeLineNosInOutput = "IncludeLineNosInOutput";
        private const string c_IncludePerfStats = "IncludePerfStats";
        private const string c_CaseSensitive = "CaseSensitive";
        private const string c_SearchSubfolders = "SearchSubfolders";
        private const string c_OnlySearchFileNames = "OnlySearchFileNames";
        private const string c_IncludeOffice = "IncludeOffice";
        private const string c_SearchTerms = "SearchTerms";
        private const string c_ExcludeTerms = "ExcludeTerms";
 
        public bool IncludeLineNosInOutput
        {
            get
            {
                return m_IncludeLineNosInOutput;
            }
            set
            {
                m_IncludeLineNosInOutput = value;
            }
        }

        public bool IncludePerfStats
        {
            get
            {
                return m_IncludePerfStats;
            }
            set
            {
                m_IncludePerfStats = value;
            }
        }

        public bool CaseSensitive
        {
            get
            {
                return m_CaseSensitive;
            }
            set
            {
                m_CaseSensitive = value;
            }
        }

        public bool IncludeOffice
        {
            get { return m_IncludeOffice; }
            set { m_IncludeOffice = value; }
        }

        public bool SearchSubfolders
        {
            get
            {
                return m_SearchSubfolders;
            }
            set
            {
                m_SearchSubfolders = value;
            }
        }

        public bool OnlySearchFileNames
        {
            get
            {
                return m_OnlySearchFileNames;
            }
            set
            {
                m_OnlySearchFileNames = value;
            }
        }

        public string SearchFolder
        {
            get
            {
                return m_SearchFolder;
            }
            set
            {
                m_SearchFolder = value;
            }
        }

        public string FileTypeFilter
        {
            get
            {
                return m_FileTypeFilter;
            }
            set
            {
                m_FileTypeFilter = value;
            }
        }

        public string FileExcludeFilter
        {
            get
            {
                return m_FileExcludeFilter;
            }
            set
            {
                m_FileExcludeFilter = value;
            }
        }

        public string[] SearchTerms
        {
            get
            {
                return m_SearchTerms;
            }
            set
            {
                m_SearchTerms = value;
            }
        }

        public string[] ExcludeTerms
        {
            get
            {
                return m_ExcludeTerms;
            }
            set
            {
                m_ExcludeTerms = value;
            }
        }

        public SearchParameters()
        {
            reg = Registry.CurrentUser;
            reg.CreateSubKey(c_RegKeyName);
            reg = Registry.CurrentUser.OpenSubKey(c_RegKeyName, true);
            LoadFromRegistry();
        }

        ~SearchParameters()
        {
            if (reg != null)
            {
                reg.Close();
            }
        }

        public override void SaveToRegistry()
        {
            reg.SetValue(c_SearchFolder, SearchFolder);
            reg.SetValue(c_FileTypeFilter, FileTypeFilter);
            reg.SetValue(c_FileExcludeFilter, FileExcludeFilter);
            reg.SetValue(c_IncludeLineNosInOutput, IncludeLineNosInOutput.ToString());
            reg.SetValue(c_IncludePerfStats, IncludePerfStats.ToString());
            reg.SetValue(c_CaseSensitive, CaseSensitive.ToString());
            reg.SetValue(c_SearchSubfolders, SearchSubfolders.ToString());
            reg.SetValue(c_OnlySearchFileNames, OnlySearchFileNames.ToString());
            reg.SetValue(c_IncludeOffice, IncludeOffice.ToString());
            SaveTerms(c_SearchTerms, ref m_SearchTerms);
            SaveTerms(c_ExcludeTerms, ref m_ExcludeTerms);
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

        public static Boolean DefaultOnlyFilesState()
        {
            return false;
        }
        public static string DefaultFileTypeFilter()
        {
            return "*.*";
        }

        public static Boolean DefaultCaseSensitiveState()
        {
            return false;
        }

        public static Boolean DefaultIncludeSubfoldersState()
        {
            return true;
        }

        public static Boolean DefaultIncludeLineNosState()
        {
            return false;
        }

        public static Boolean DefaultIncludePerfStatsState()
        {
            return false;
        }

        public static Boolean DefaultIncludeOfficeState()
        { return false; }

        public static string[] DefaultExcludeList()
        {
            return Util.EmptyStringArray();
        }

        public static string[] DefaultSearchTermList()
        {
            return Util.EmptyStringArray();
        }

        public static string DefaultSearchFolder()
        {
            return @"C:\";
        }

        public static string DefaultExcludeFilter()
        {
            return "";
        }

        public override void LoadFromRegistry()
        {
            SearchFolder = reg.GetValue(c_SearchFolder, DefaultSearchFolder()).ToString();
            FileTypeFilter = reg.GetValue(c_FileTypeFilter, DefaultFileTypeFilter()).ToString();
            FileExcludeFilter = reg.GetValue(c_FileExcludeFilter, DefaultExcludeFilter()).ToString();
            IncludeLineNosInOutput = (true.ToString() == (reg.GetValue(c_IncludeLineNosInOutput, DefaultIncludeLineNosState().ToString()).ToString()));
            IncludePerfStats = (true.ToString() == (reg.GetValue(c_IncludePerfStats, DefaultIncludePerfStatsState().ToString()).ToString()));
            CaseSensitive = (true.ToString() == (reg.GetValue(c_CaseSensitive, DefaultCaseSensitiveState().ToString()).ToString()));
            SearchSubfolders = (true.ToString() == (reg.GetValue(c_SearchSubfolders, DefaultIncludeSubfoldersState().ToString()).ToString()));
            OnlySearchFileNames = (true.ToString() == (reg.GetValue(c_OnlySearchFileNames,DefaultOnlyFilesState().ToString()).ToString()));
            IncludeOffice = (true.ToString() == (reg.GetValue(c_IncludeOffice, DefaultIncludeOfficeState().ToString()).ToString()));
            SearchTerms = LoadTerms(c_SearchTerms);
            ExcludeTerms = LoadTerms(c_ExcludeTerms);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("SearchFolder", this.SearchFolder);
            info.AddValue("FileTypeFilter", this.FileTypeFilter);
            info.AddValue("FileExcludeFilter", this.FileExcludeFilter);
            info.AddValue("IncludeLineNosInOutput", this.IncludeLineNosInOutput);
            info.AddValue("IncludePerfStats", this.IncludePerfStats);
            info.AddValue("CaseSensitive", this.CaseSensitive);
            info.AddValue("SearchSubfolders", this.SearchSubfolders);
            info.AddValue("OnlySearchFileNames", this.OnlySearchFileNames);
            info.AddValue("SearchTerms", this.SearchTerms);
            info.AddValue("ExcludeTerms", this.ExcludeTerms);            
        }

        public override void Owner(SerializationInfo info, StreamingContext ctxt)
        {
            this.SearchFolder = (string)info.GetValue("SearchFolder", typeof(string));
            this.FileTypeFilter = (string)info.GetValue("FileTypeFilter", typeof(string));
            this.FileExcludeFilter = (string)info.GetValue("FileExcludeFilter", typeof(string));
            this.IncludeLineNosInOutput = (Boolean)info.GetValue("IncludeLineNosInOutput", typeof(Boolean));
            this.IncludePerfStats = (Boolean)info.GetValue("IncludePerfStats", typeof(Boolean));
            this.CaseSensitive = (Boolean)info.GetValue("CaseSensitive", typeof(Boolean));
            this.SearchSubfolders = (Boolean)info.GetValue("SearchSubfolders", typeof(Boolean));
            this.OnlySearchFileNames = (Boolean)info.GetValue("OnlySearchFileNames", typeof(Boolean));
            this.SearchTerms = (string[])info.GetValue("SearchTerms", typeof(string[]));
            this.ExcludeTerms = (string[])info.GetValue("ExcludeTerms", typeof(string[]));
        }

        public SearchParameters(SerializationInfo info, StreamingContext ctxt)
        {
            this.SearchFolder = SafeGetStr(ref info, "SearchFolder", DefaultSearchFolder());
            this.FileTypeFilter = SafeGetStr(ref info, "FileTypeFilter", DefaultFileTypeFilter());
            this.FileExcludeFilter = SafeGetStr(ref info, "FileExcludeFilter", DefaultExcludeFilter());
            this.IncludeLineNosInOutput = SafeGetBool(ref info, "IncludeLineNosInOutput", DefaultIncludeLineNosState());
            this.IncludePerfStats = SafeGetBool(ref info, "IncludePerfStats", DefaultIncludePerfStatsState());
            this.CaseSensitive = SafeGetBool(ref info, "CaseSensitive", DefaultCaseSensitiveState());
            this.SearchSubfolders = SafeGetBool(ref info, "SearchSubFolders", DefaultIncludeSubfoldersState());
            this.OnlySearchFileNames = SafeGetBool(ref info, "OnlySearchFileNames", DefaultOnlyFilesState());
            this.SearchTerms = SafeGetStrArry(ref info, "SearchTerms", Util.EmptyStringArray());
            this.ExcludeTerms = SafeGetStrArry(ref info, "ExcludeTerms", Util.EmptyStringArray());
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
