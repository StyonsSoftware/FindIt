/*
This class represents the various options that we can define on a BBEC build.
It is mostly a big collection of public properties, to hold folder names
and other machine-specific information.

It also includes functionality to save the preferences *to* the registry, and
to load them back *from* the registry.
//*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Findit
{
    class GUIPreferences
    {
        private RegistryKey reg;
        private string m_SearchFolder = "";
        private string m_FileTypeFilter = "";
        private string m_FileExcludeFilter = "";
        private string m_CustomEditorExe = "";
        private bool m_IncludeLineNosInOutput = false;
        private bool m_IncludePerfStats = false;
        private bool m_CaseSensitive = false;
        private bool m_SearchSubfolders = true;
        private string[] m_SearchTerms = { "" };
        private string[] m_ExcludeTerms = { "" };
        private string[] m_RecentSearchFolders = { "" };

        private const string c_RegKeyName = @"Software\BlackbaudUtility\FindIt";
        private const string c_SearchFolder = "SearchFolder";
        private const string c_FileTypeFilter = "FileTypeFilter";
        private const string c_FileExcludeFilter = "FileExcludeFilter";
        private const string c_CustomEditorExe = "CustomEditorExe";
        private const string c_IncludeLineNosInOutput = "IncludeLineNosInOutput";
        private const string c_IncludePerfStats = "IncludePerfStats";
        private const string c_CaseSensitive = "CaseSensitive";
        private const string c_SearchSubfolders = "SearchSubfolders";
        private const string c_SearchTerms = "SearchTerms";
        private const string c_ExcludeTerms = "ExcludeTerms";
        private const string c_RecentSearchFolders = "RecentSearchFolders";
 
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

        public GUIPreferences()
        {
            reg = Registry.CurrentUser;
            reg.CreateSubKey(c_RegKeyName);
            reg = Registry.CurrentUser.OpenSubKey(c_RegKeyName, true);
            LoadFromRegistry();
        }

        ~GUIPreferences()
        {
            reg.Close();
        }

        public void SaveToRegistry()
        {
            reg.SetValue(c_SearchFolder, SearchFolder);
            reg.SetValue(c_FileTypeFilter, FileTypeFilter);
            reg.SetValue(c_FileExcludeFilter, FileExcludeFilter);
            reg.SetValue(c_IncludeLineNosInOutput, IncludeLineNosInOutput.ToString());
            reg.SetValue(c_IncludePerfStats, IncludePerfStats.ToString());
            reg.SetValue(c_CaseSensitive, CaseSensitive.ToString());
            reg.SetValue(c_SearchSubfolders, SearchSubfolders.ToString());
            reg.SetValue(c_CustomEditorExe, CustomEditorExe);
            SaveTerms(c_SearchTerms, ref m_SearchTerms);
            SaveTerms(c_ExcludeTerms, ref m_ExcludeTerms);
            SaveTerms(c_RecentSearchFolders, ref m_RecentSearchFolders);
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

        public void LoadFromRegistry()
        {
            SearchFolder = reg.GetValue(c_SearchFolder, @"C:\").ToString();
            FileTypeFilter = reg.GetValue(c_FileTypeFilter, "*.*").ToString();
            FileExcludeFilter = reg.GetValue(c_FileExcludeFilter, "*.*").ToString();
            CustomEditorExe = reg.GetValue(c_CustomEditorExe, @"c:\windows\notepad.exe").ToString();
            IncludeLineNosInOutput = (true.ToString() == (reg.GetValue(c_IncludeLineNosInOutput, false.ToString()).ToString()));
            IncludePerfStats = (true.ToString() == (reg.GetValue(c_IncludePerfStats, false.ToString()).ToString()));
            CaseSensitive = (true.ToString() == (reg.GetValue(c_CaseSensitive, false.ToString()).ToString()));
            SearchSubfolders = (true.ToString() == (reg.GetValue(c_SearchSubfolders, true.ToString()).ToString()));
            SearchTerms = LoadTerms(c_SearchTerms);
            ExcludeTerms = LoadTerms(c_ExcludeTerms);
            RecentSearchFolders = LoadTerms(c_RecentSearchFolders);
        }
    }
}
