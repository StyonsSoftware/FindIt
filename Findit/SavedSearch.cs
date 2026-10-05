/*
This is the on-disk shape of a ".fit" saved search file.

It is deliberately a plain data holder with no behavior and no base class.  That is the
whole point: it is the only type the deserializer is ever allowed to construct, so a
hostile .fit file has nothing to work with.  See Serializer.cs for the history.

Keep it that way.  Do not give this class a base class, an interface, or a member whose
type is anything other than a string, a bool, or an array of those.
//*/
using System;

namespace Findit
{
    public class SavedSearch
    {
        public string SearchFolder = "";
        public string FileTypeFilter = "";
        public string FileExcludeFilter = "";
        public bool IncludeLineNosInOutput;
        public bool IncludePerfStats;
        public bool CaseSensitive;
        public bool SearchSubfolders = true;
        public bool OnlySearchFileNames;
        public bool IncludeOffice;
        public string[] SearchTerms = Util.EmptyStringArray();
        public string[] ExcludeTerms = Util.EmptyStringArray();

        public static SavedSearch From(SearchParameters sp)
        {
            if (sp == null) throw new ArgumentNullException("sp");

            SavedSearch result = new SavedSearch();
            result.SearchFolder = sp.SearchFolder;
            result.FileTypeFilter = sp.FileTypeFilter;
            result.FileExcludeFilter = sp.FileExcludeFilter;
            result.IncludeLineNosInOutput = sp.IncludeLineNosInOutput;
            result.IncludePerfStats = sp.IncludePerfStats;
            result.CaseSensitive = sp.CaseSensitive;
            result.SearchSubfolders = sp.SearchSubfolders;
            result.OnlySearchFileNames = sp.OnlySearchFileNames;
            result.IncludeOffice = sp.IncludeOffice;
            result.SearchTerms = sp.SearchTerms;
            result.ExcludeTerms = sp.ExcludeTerms;
            return result;
        }

        public SearchParameters ToSearchParameters()
        {
            //anything the file left out keeps whatever default the properties already hold
            SearchParameters result = new SearchParameters();
            result.SearchFolder = NotNull(SearchFolder, SearchParameters.DefaultSearchFolder());
            result.FileTypeFilter = NotNull(FileTypeFilter, SearchParameters.DefaultFileTypeFilter());
            result.FileExcludeFilter = NotNull(FileExcludeFilter, SearchParameters.DefaultExcludeFilter());
            result.IncludeLineNosInOutput = IncludeLineNosInOutput;
            result.IncludePerfStats = IncludePerfStats;
            result.CaseSensitive = CaseSensitive;
            result.SearchSubfolders = SearchSubfolders;
            result.OnlySearchFileNames = OnlySearchFileNames;
            result.IncludeOffice = IncludeOffice;
            result.SearchTerms = NotNull(SearchTerms);
            result.ExcludeTerms = NotNull(ExcludeTerms);
            return result;
        }

        private static string NotNull(string s, string defaultValue)
        {
            return s ?? defaultValue;
        }

        private static string[] NotNull(string[] arry)
        {
            return arry ?? Util.EmptyStringArray();
        }
    }
}
