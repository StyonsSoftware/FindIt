﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Findit
{
    public partial class frmMain : Form
    {
        GrepTool.Grepper gp;
        Thread searchThread;
        GrepTool.Grepper.UserParameters uparms;
        Findit.Registration regInfo;
        Boolean g_Crippled = false;
        Boolean AlreadyQuit = false;
        Boolean FinishedLoading = false;

        private int LastReadResult = 0;
        private const int c_RecentSearchCutoff = 10;
        private const int c_MaxRecentSearches = 10;
        const int c_buffer = 20;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            splitCont.Panel2Collapsed = true;
            LoadSearchParameters();
            LoadGuiPreferences();
            HighlightInvalidFolder();
            rtbSearchTerms.Select();
            openFileInCustomEditorToolStripMenuItem.ToolTipText = "Opens the selected file in an editor of your choice" +
                Environment.NewLine + "To change the custom editor, use the Tools->Options menu";

            //permit passing a saved search as a start parameter.
            //this facilitates the ability to double-click a ".fit" file and
            //launch the application with the saved terms
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            if((1 < cmdLineArgs.Length) && (System.IO.File.Exists(cmdLineArgs[0])))
            {
                LoadSavedSearch(cmdLineArgs[1]);
            }

            ApplyLicenseRules();
            SetSearchTermsWidth();
            FinishedLoading = true;
            ManuallyArrangeOutputArea();
            SetSearchTermsWidth();
        }

        private void ApplyLicenseRules()
        {
            regInfo = new Registration();

            if (regInfo.PaidFor())
            {
                SetPerformanceCrippling(false);
            }
            else
            {
                if (regInfo.WithinTrialPeriod())
                {
                    BegForMoney();
                    SetPerformanceCrippling(false);
                }
                else
                {
                    BegForMoney();
                    SetPerformanceCrippling(!regInfo.PaidFor());
                }
            }
        }

        private void SetPerformanceCrippling(Boolean IsCrippled)
        {
            g_Crippled = IsCrippled;
        }

        private void BegForMoney()
        {
            PurchaseOptions regForm = new PurchaseOptions();
            regForm.StartPosition = FormStartPosition.CenterParent;
            regForm.ShowDialog();
        }

        private void ManuallyArrangeOutputArea()
        {
            splitCont.Left = 0;
            splitCont.Width = panMid.Width;
            splitCont.Height = panMid.Height - 150;
            splitCont.SplitterDistance = panMid.Width / 2;
        }

        private int SizeOfSearchTermsBox(ref RichTextBox searchTermsBox, int minimumSize, int maximumSize, int buffer)
        {
            /*
             * Size of the search terms box:
             * Minimum = width of the label above it + [buffer]
             * Maximum = up to edge of the form - [buffer]
             * Preferred = width of the widest text item in the box + [buffer]
            */

            //int minimumSize = lblSearchTerms.Width + buffer;
            //int maximumSize = tpgBasic.Width - rtbSearchTerms.Left - buffer;
            
            //calculate the "preferred size"
            int preferredSize = 0;
            foreach (string s in rtbSearchTerms.Lines)
            {
                preferredSize = Math.Max(preferredSize, Util.GetPixelWidthOfFormattedText(s, searchTermsBox.Font, searchTermsBox.Width)) + buffer;
            }

            if (preferredSize > maximumSize)
            {
                return maximumSize;
            }
            else if (preferredSize < minimumSize)
            {
                return minimumSize;
            }
            else
            {
                return preferredSize;
            }
        }

        private SearchParameters SearchParametersFromUI()
        {
            SearchParameters sp = new SearchParameters();
            sp.FileTypeFilter = txbFileType.Text;
            sp.FileExcludeFilter = txbFileExclude.Text;
            sp.IncludeLineNosInOutput = cbLineNos.Checked;
            sp.SearchFolder = cboSearchFolders.Text;
            sp.IncludeLineNosInOutput = cbLineNos.Checked;
            sp.SearchSubfolders = cbIncludeSubfolders.Checked;
            sp.OnlySearchFileNames = cbOnlyFiles.Checked;
            sp.CaseSensitive = cbCaseSensitive.Checked;
            sp.IncludePerfStats = cbPerfStats.Checked;
            sp.SearchTerms = rtbSearchTerms.Lines;
            sp.ExcludeTerms = rtbExcludes.Lines;
            return sp;
        }

        private GUIPreferences GuiPreferencesFromUI()
        {
            GUIPreferences gp = new GUIPreferences();
            gp.RecentSavedSearches = Util.MenuChildrenToStrArry(ref recentSearchesToolStripMenuItem, false);
            gp.RecentSearchFolders = Util.ComboToStrArry(ref cboSearchFolders, c_RecentSearchCutoff);
            if ((gp.RecentSearchFolders.Length == 1) && (gp.RecentSearchFolders[0].Length == 0))
            {
                gp.RecentSearchFolders = null;
            }

            return gp;
        }

        private void SavePreferences()
        {
            SearchParameters sp = SearchParametersFromUI();
            sp.SaveToRegistry();

            GUIPreferences gp = GuiPreferencesFromUI();
            gp.SaveToRegistry();
        }

        private void LoadSearchParameters()
        {
            SearchParameters sp = new SearchParameters();
            UIFromSearchParameters(sp); 
        }

        private void LoadGuiPreferences()
        {
            GUIPreferences gp = new GUIPreferences();
            UIFromGuiPreferences(gp);
        }

        private void UIFromSearchParameters(SearchParameters sp)
        {
            //make the UI match a preferences object
            txbFileType.Text = sp.FileTypeFilter;
            txbFileExclude.Text = sp.FileExcludeFilter;
            cbLineNos.Checked = sp.IncludeLineNosInOutput;
            cbIncludeSubfolders.Checked = sp.SearchSubfolders;
            cbOnlyFiles.Checked = sp.OnlySearchFileNames;
            cbCaseSensitive.Checked = sp.CaseSensitive;
            cbPerfStats.Checked = sp.IncludePerfStats;
            rtbSearchTerms.Lines = sp.SearchTerms;
            rtbExcludes.Lines = sp.ExcludeTerms;
            cboSearchFolders.Text = sp.SearchFolder;
        }

        private void UIFromGuiPreferences(GUIPreferences gp)
        {
            Util.StrArryToComboItems(ref cboSearchFolders, gp.RecentSearchFolders, false);
            Util.StrArryToMenuChildren(ref recentSearchesToolStripMenuItem, gp.RecentSavedSearches, false);
            //each of the menu items must have an event handler assigned
            foreach (ToolStripItem ts in recentSearchesToolStripMenuItem.DropDownItems)
            {
                ts.Click += LoadRecentlyUsedSearch;
            }
        }

        private void ConductSearch()
        {
            runSearchToolStripMenuItem.Visible = false;
            cancelSearchToolStripMenuItem.Visible = !runSearchToolStripMenuItem.Visible;
            lbResults.Items.Clear();
            LastReadResult = 0;
            splitCont.Panel2Collapsed = true;
            uparms = new GrepTool.Grepper.UserParameters();
            uparms.SearchPath = cboSearchFolders.Text;
            uparms.SearchStrings = rtbSearchTerms.Lines;
            uparms.Recurse = cbIncludeSubfolders.Checked;
            uparms.Verbosity = 0;
            uparms.CaseSensitive = cbCaseSensitive.Checked;
            uparms.IncludeLineNumbers = cbLineNos.Checked;
            uparms.Remind = true;
            uparms.SearchExtension = txbFileType.Text;
            uparms.SearchExcludeFiles = txbFileExclude.Text;
            uparms.ShowPerfStats = cbPerfStats.Checked;
            uparms.AbsentStrings = rtbExcludes.Lines;
            uparms.Crippled = g_Crippled;
            uparms.OnlyFileNames = cbOnlyFiles.Checked;
            
            gp = new GrepTool.Grepper(uparms);
            searchThread = new Thread(gp.Search);
            searchThread.IsBackground = true;
            searchThread.Start();

            //at this point, the thread is running.  just watch it and print what's going on
            timerRefreshGUI.Enabled = true;
            while (SearchIsActive())
            {
                RefreshGUI();
                System.Threading.Thread.Sleep(0);
                Application.DoEvents();
            }
            //one final printout in case we missed anything
            RefreshGUI();
            timerRefreshGUI.Enabled = false;

            if (cbPerfStats.Checked)
            {
                PrintPerformanceStats();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string validmsg = "";
            if (ValidSearchTerms(ref validmsg))
            {
                ConductSearch();
                AddToRecentSearches(cboSearchFolders.Text.Trim());
            }
            else
            {
                validmsg += Environment.NewLine + "Search anyway?";
                if (DialogResult.Yes == MessageBox.Show(validmsg,"Confirm",MessageBoxButtons.YesNo))
                {
                    ConductSearch();
                    AddToRecentSearches(cboSearchFolders.Text.Trim());
                }
            }
        }

        private Boolean ValidSearchTerms(ref string msg)
        {
            //either a search term has been specified or an exclude term has been specified,
            //or they are just searching for file names and specified
            Boolean result =
                   (0 < rtbSearchTerms.Lines.Length)
                || (0 < rtbExcludes.Lines.Length)
                || (
                    (cbOnlyFiles.Checked) &&
                    (
                     (0 < txbFileType.Text.Length)
                     ||
                     (0 < txbFileExclude.Text.Length)
                    )
                   );
            if (!result)
            {
                msg = "No valid search terms have been specified!";
            }
            else
            {
                msg = "";
            }
            return result;
        }

        private void SetEnabledStatesDuringSearch(Boolean SearchIsInProgress)
        {
            //some changes will not take effect until the next search begins.
            //Indicate this by disabling them while the search is in progress.
            cboSearchFolders.Enabled = !SearchIsInProgress;
            btnBrowse.Enabled = !SearchIsInProgress;
            cbLineNos.Enabled = !SearchIsInProgress;
            cbIncludeSubfolders.Enabled = !SearchIsInProgress;
            cbCaseSensitive.Enabled = !SearchIsInProgress;
            rtbSearchTerms.Enabled = !SearchIsInProgress;
            rtbExcludes.Enabled = !SearchIsInProgress;
            txbFileType.Enabled = !SearchIsInProgress;
            txbFileExclude.Enabled = !SearchIsInProgress;
        }

        private Boolean SearchIsActive()
        {
            return (
                    (searchThread != null)
                    &&
                    (
                     (searchThread.ThreadState == System.Threading.ThreadState.Running)
                     ||
                     (searchThread.ThreadState == System.Threading.ThreadState.Background)
                    )
                   );
        }

        private void RefreshSearchResults()
        {
            if ((gp != null) && (LastReadResult < gp.SearchResultCount))
            {
                for (int i = LastReadResult; i < gp.SearchResultCount; ++i)
                {
                    if (uparms.IncludeLineNumbers)
                    {
                        writeoutput(gp.SearchResults[i].FileName + " @ " + gp.SearchResults[i].LineNumber.ToString());
                    }
                    else
                    {
                        writeoutput(gp.SearchResults[i].FileName);
                    }
                }
                LastReadResult = gp.SearchResultCount;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderClick(ref cboSearchFolders);
        }

        private void FolderClick(ref ComboBox DisplayBox)
        {
            //show a folder browser dialog & tie it to whatever textbox they gave us
            var dlg = new FolderBrowserDialog();
            if (System.IO.Directory.Exists(DisplayBox.Text))
            {
                dlg.SelectedPath = DisplayBox.Text;
            }
            else
            {
                dlg.SelectedPath = "c:\\";
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DisplayBox.Text = dlg.SelectedPath;
            }

        }

        private void HighlightInvalidFolder()
        {
            if (System.IO.Directory.Exists(cboSearchFolders.Text))
            {
                cboSearchFolders.BackColor = txbFileType.BackColor;
                lblInvalidNotice.Visible = false;
            }
            else
            {
                cboSearchFolders.BackColor = Color.Yellow;
                lblInvalidNotice.Visible = true;
            }
        }

        private void RefreshGUI()
        {
            //make the GUI reflect the reality of whatever is going on right now
            RefreshSearchResults();
            
            btnSearch.Enabled = !SearchIsActive();

            if (SearchIsActive())
            {
                btnSearch.Enabled = false;
                btnSearch.Text = "Working...";
                runSearchToolStripMenuItem.Visible = false;
                cancelSearchToolStripMenuItem.Visible = true;
                lblProgress.Text = gp.LastSearchedFolder;
                lblCrippled.Visible = g_Crippled;
                btnClear.Enabled = false;
            }
            else
            {
                btnSearch.Enabled = true;
                btnSearch.Text = "Search";
                runSearchToolStripMenuItem.Visible = true;
                cancelSearchToolStripMenuItem.Visible = false;
                rtbSearchTerms.Enabled = true;
                lblProgress.Text = "Search complete!";
                lblCrippled.Visible = false;
                btnClear.Enabled = true;
            }
            SetEnabledStatesDuringSearch(!btnSearch.Enabled);
            btnCancel.Enabled = !btnSearch.Enabled;
            Application.DoEvents();
        }

        private void timerRefreshGUI_Tick(object sender, EventArgs e)
        {
            RefreshGUI();
        }

        private void PrintPerformanceStats()
        {
            try
            {
                Int64 fps = 0;
                Int64 lps = 0;
                if (0 < gp.PerformanceStats.ElapsedSeconds)
                {
                    fps = (Int64)Math.Round(gp.PerformanceStats.FilesSearched / gp.PerformanceStats.ElapsedSeconds);
                    lps = (Int64)Math.Round(gp.PerformanceStats.LinesSearched / gp.PerformanceStats.ElapsedSeconds);
                }
                writeoutput("");
                writeoutput("Performance stats:");
                writeoutput("-------------------------------");
                writeoutput("Elapsed seconds: " + gp.PerformanceStats.ElapsedSeconds.ToString());
                writeoutput("Files searched: " + gp.PerformanceStats.FilesSearched.ToString());
                writeoutput("Lines searched: " + gp.PerformanceStats.LinesSearched.ToString());
                writeoutput("Files per second: " + fps.ToString());
                writeoutput("Lines per second: " + lps.ToString());
                writeoutput("Matches: " + gp.PerformanceStats.Matches.ToString());
                writeoutput("Skipped (binary): " + gp.PerformanceStats.Skipped.ToString());
                writeoutput("Errors: " + gp.PerformanceStats.Errors.ToString());
                writeoutput("");
            }
            catch (Exception e)
            {
                writeoutput("Error trying to report performance stats: '" + e.Message + "'");
            }
        }

        private void writeoutput(string WhatToWrite)
        {
            lbResults.Items.Add(WhatToWrite);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelSearch();
        }

        private void CancelSearch()
        {
            if(SearchIsActive())
            {
                searchThread.Abort();
            }
            RefreshGUI();
        }

        private Boolean QuitApplication()
        {
            SavePreferences();
            if (SearchIsActive())
            {
                if (DialogResult.Yes == MessageBox.Show("A search is in progress.  Really quit?",
                    "Are you sure?", MessageBoxButtons.YesNo))
                {
                    CancelSearch();
                    Application.Exit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Application.Exit();
                return true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitApplication();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox bx = new AboutBox();
            bx.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosing fires twice, but we only want to run the quit stuff once
            if (AlreadyQuit)
            {
                e.Cancel = false;
            }
            else
            {
                AlreadyQuit = true;
                e.Cancel = !QuitApplication();
            }
        }

        private void RefreshExcerptPane(string filename, Int64 linenumber, ref RichTextBox rtb, string[] highlightwords)
        {
            //Display an excerpt of the selected file, centered on the specified line number
            rtb.Clear();
            Int64 LastReadLine = 1;
            Int64 StartPoint = linenumber - 10;
            Int64 EndPoint = linenumber + 10;
            Int64 maxCutoff = 1000;
            Int64 iters = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(filename);
            string currentLine = "";
            do
            {
                currentLine = reader.ReadLine();
                if((currentLine == null) || (-1 < currentLine.IndexOf("\0\0\0\0\0\0\0")))
                {
                    rtb.Clear();
                    System.IO.FileInfo finfo = new System.IO.FileInfo(filename);
                    System.Diagnostics.FileVersionInfo myFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(filename);
                    rtb.AppendText("This is a binary file.  Preview is not available.");
                    rtb.AppendText(Environment.NewLine + "File size (in MB): " + Math.Round(finfo.Length / 1048576.0,4).ToString());
                    rtb.AppendText(Environment.NewLine + "File version     : " + myFileVersionInfo.FileVersion);
                    break;
                }
                if ((currentLine != null) && (LastReadLine > StartPoint) && (LastReadLine < EndPoint))
                {
                    rtb.AppendText(currentLine + Environment.NewLine);
                }
                LastReadLine++;

                if (LastReadLine > EndPoint)
                {
                    break;
                }
                if (++iters > (maxCutoff + linenumber))
                {
                    MessageBox.Show("Looped more than " + maxCutoff.ToString() + " in 'RefreshExceptPane' with these arguments:"
                        + Environment.NewLine + "filename = '" + filename + "'"
                        + Environment.NewLine + "linenumber = " + linenumber.ToString());
                    break;
                }
            } while (!reader.EndOfStream);
            foreach (string s in highlightwords)
            {
                Util.HighlightWordInRtb(ref rtb, s);
            }
            reader.Close();
            splitCont.Panel2Collapsed = false;
        }

        private void lbResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedfname = gp.SearchResults[lbResults.SelectedIndex].FileName;
                if (System.IO.File.Exists(selectedfname))
                {
                    Int64 CenterOnThisLine = gp.SearchResults[lbResults.SelectedIndex].LineNumber;
                    RefreshExcerptPane(selectedfname, CenterOnThisLine, ref rtbExcerpt, rtbSearchTerms.Lines);
                }
            }
            catch
            {
                //do nothing
            }
        }

        private void lbResults_DoubleClick(object sender, EventArgs e)
        {
            Util.OpenFileInCustomEditor(SelectedResultsFile(),CustomEditorExeName());
        }

        private void btnTogglePreview_Click(object sender, EventArgs e)
        {
            splitCont.Panel2Collapsed = !splitCont.Panel2Collapsed;
        }

        private void frmMain_ClientSizeChanged(object sender, EventArgs e)
        {
            if (FinishedLoading)
            {
                ManuallyArrangeOutputArea();
                SetSearchTermsWidth();
            }
        }

        private Boolean StringArraysAreTheSame(string[] arry1, string[] arry2)
        {
            if (arry1.Length != arry2.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < arry1.Length; ++i)
                {
                    if (arry2[i] != arry1[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Boolean CustomAdvancedOptions()
        {
            //returns true if the advanced options area has been customized in some way
            return (
                    (!StringArraysAreTheSame(rtbExcludes.Lines, SearchParameters.DefaultExcludeList()))
                    ||
                    (txbFileExclude.Text != SearchParameters.DefaultExcludeFilter())
                    ||
                    (cbCaseSensitive.Checked != SearchParameters.DefaultCaseSensitiveState())
                    ||
                    (cbIncludeSubfolders.Checked != SearchParameters.DefaultIncludeSubfoldersState())
                    ||
                    (cbOnlyFiles.Checked != SearchParameters.DefaultOnlyFilesState())
                    ||
                    (cbLineNos.Checked != SearchParameters.DefaultIncludeLineNosState())
                    ||
                    (cbPerfStats.Checked != SearchParameters.DefaultIncludePerfStatsState())
                   );
        }

        private void ClearOptions()
        {
            //basic options
            cboSearchFolders.Text = SearchParameters.DefaultSearchFolder();
            rtbSearchTerms.Lines = SearchParameters.DefaultSearchTermList();

            //advanced options
            rtbExcludes.Lines = SearchParameters.DefaultExcludeList();
            txbFileType.Text = SearchParameters.DefaultFileTypeFilter();
            txbFileExclude.Text = SearchParameters.DefaultExcludeFilter();
            cbCaseSensitive.Checked = SearchParameters.DefaultCaseSensitiveState();
            cbIncludeSubfolders.Checked = SearchParameters.DefaultIncludeSubfoldersState();
            cbOnlyFiles.Checked = SearchParameters.DefaultOnlyFilesState();
            cbLineNos.Checked = SearchParameters.DefaultIncludeLineNosState();
            cbPerfStats.Checked = SearchParameters.DefaultIncludePerfStatsState();

            //results area
            rtbExcerpt.Clear();
            lbResults.Items.Clear();

            tabctlSearchOptions.Refresh();
        }

        private void AddToRecentSearches(string s)
        {
            if (-1 == cboSearchFolders.Items.IndexOf(s))
            {
                cboSearchFolders.Items.Add(s);
            }
        }

        private void runSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConductSearch();
            AddToRecentSearches(cboSearchFolders.Text.Trim());
        }

        private void cancelSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SearchIsActive())
            {
                CancelSearch();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyResultsToClipboard();
        }

        private void CopyResultsToClipboard()
        {
            string copytext = "";
            foreach (string s in lbResults.Items)
            {
                copytext += (s + Environment.NewLine);
            }
            if (0 < copytext.Length)
            {
                Clipboard.SetText(copytext.Trim());
            }
            else
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private void lblProgress_Click(object sender, EventArgs e)
        {
            if(System.IO.Directory.Exists(lblProgress.Text))
            {
                System.Diagnostics.Process.Start("explorer.exe", lblProgress.Text);
            }
        }

        private void copySelectedRowToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(lbResults.Items[lbResults.SelectedIndex].ToString().Trim());
            }
            catch
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private void openFileInDefaultEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenFile(lbResults.Items[lbResults.SelectedIndex].ToString().Trim(),"");
            }
            catch
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private void openFileInCustomEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (0 < SelectedResultsFile().Length)
            {
                Util.OpenFileInCustomEditor(SelectedResultsFile(),CustomEditorExeName());
            }
            else
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private string CustomEditorExeName()
        {
            GUIPreferences gp = new GUIPreferences();
            return gp.CustomEditorExe;
        }

        private Boolean RunSavedSearchesAfterLoad()
        {
            GUIPreferences gp = new GUIPreferences();
            return gp.RunSearchesAfterLoad;
        }

        private void openEnclosingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (0 < SelectedResultsFile().Length)
            {
                Util.OpenEnclosingFolder(SelectedResultsFile());
            }
            else
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private string SelectedResultsFile()
        {
            if (lbResults.SelectedItem != null)
            {
                return lbResults.SelectedItem.ToString();
            }
            else
            {
                return "";
            }
        }

        private void oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOptions();
        }

        private void ShowOptions()
        {
            Config cf = new Config();
            cf.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if(DialogResult.Yes == MessageBox.Show("Really reset your filter options?","Confirm",MessageBoxButtons.YesNo))
            {
                ClearOptions();
            }
        }

        private Brush GetBackgroundBrush(int TabIdx)
        {
            return new SolidBrush(tpgAdvanced.BackColor);
        }

        private Brush GetForegroundBrush(int TabIdx)
        {
            return new SolidBrush(tpgAdvanced.ForeColor);
        }

        private Font GetTabCaptionFont(int TabIdx, Font currFont)
        {
            //return the font that is correct for the given tab index
            //the "correct" font is based on the current font of that tab.
            Font resultFont = new Font(currFont,currFont.Style);
            switch (TabIdx)
            {
                case 0:
                    resultFont = new Font(currFont, FontStyle.Bold); break;
                case 1:
                    if(CustomAdvancedOptions())
                    {
                        resultFont = new Font(currFont, FontStyle.Italic | FontStyle.Bold);
                    }
                    break;
                default:
                    break;
            }
            return resultFont;
        }

        private void tabctlSearchOptions_DrawItem(object sender, DrawItemEventArgs e)
        {
            //this event is the plumbing for the "Advanced Options" tab to be drawn in
            //italics whenever non-default options are set on that tab.
            //see the "GetTabCaptionFont" function for the important decision-making
            Font TabFont;
            Brush BackBrush = GetBackgroundBrush(e.Index);
            Brush ForeBrush = GetForegroundBrush(e.Index);
            TabFont = GetTabCaptionFont(e.Index, e.Font);
            string TabName = this.tabctlSearchOptions.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(BackBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X - 2, r.Y + 3, r.Width + 5, r.Height - 3);
            e.Graphics.DrawString(TabName, TabFont, ForeBrush, r, sf);
            
            //Cleanup
            sf.Dispose();
            TabFont.Dispose();
            BackBrush.Dispose();
            BackBrush.Dispose();
            ForeBrush.Dispose();
        }

        private void SaveCurrentSearch()
        {
            //save by seriallizing the preferences object
            SaveFileDialog svdg = new SaveFileDialog();
            svdg.Filter = "FindIt files (*.fit)|*.fit|All files (*.*)|*.*";
            svdg.FilterIndex = 1;
            if (svdg.ShowDialog() == DialogResult.OK)
            {
                Serializer s = new Serializer();
                s.SerializeObject(svdg.FileName, SearchParametersFromUI());
                AddSavedSearchToRecentSearches(svdg.FileName, c_MaxRecentSearches);
            }
        }

        private void PromptForSavedSearch()
        {
            OpenFileDialog opdg = new OpenFileDialog();
            opdg.Title = "Choose a saved search";
            opdg.InitialDirectory = "c:\\";
            opdg.Filter = "FindIt files (*.fit)|*.fit|All files (*.*)|*.*";
            opdg.FilterIndex = 1;
            opdg.RestoreDirectory = true;

            if (opdg.ShowDialog() == DialogResult.OK)
            {
                LoadSavedSearch(opdg.FileName);
                AddSavedSearchToRecentSearches(opdg.FileName,c_MaxRecentSearches);
            }
        }

        private void LoadSavedSearch(string savedsearchfilename)
        {
            //do a few gymnastics to avoid losing the "recent locations" when we reset the combo box to
            //the contents of the loaded search
            if (System.IO.File.Exists(savedsearchfilename))
            {
                string[] recentlocations = Util.ComboToStrArry(ref cboSearchFolders, c_RecentSearchCutoff);
                Serializer s = new Serializer();
                UIFromSearchParameters(s.DeSerializeObject(savedsearchfilename));
                string loadedlocation = cboSearchFolders.Text;
                foreach (string recentloc in recentlocations)
                {
                    AddToRecentSearches(recentloc);
                }
                AddToRecentSearches(loadedlocation);
                cboSearchFolders.SelectedIndex = cboSearchFolders.Items.IndexOf(loadedlocation);

                if (RunSavedSearchesAfterLoad())
                {
                    ConductSearch();
                }
            }
        }

        private void saveThisSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentSearch();
        }

        private void loadASavedSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptForSavedSearch();
            tabctlSearchOptions.Refresh();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseOptions regForm = new PurchaseOptions();
            regForm.StartPosition = FormStartPosition.CenterParent;
            regForm.ShowDialog();
            SetPerformanceCrippling(!regInfo.PaidFor());
        }

        private void AddSavedSearchToRecentSearches(string savedsearch, int maxSearches)
        {
            //add a sub-menu item to the "File->Recent searches menu item" with the recent search file name

            //prevent duplicates
            Boolean alreadyExists = false;
            foreach (ToolStripMenuItem ts in recentSearchesToolStripMenuItem.DropDownItems)
            {
                if (ts.Text == savedsearch)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                recentSearchesToolStripMenuItem.DropDownItems.Add(savedsearch);
                //add an event handler so that something happens when they click the new menu item
                recentSearchesToolStripMenuItem.DropDownItems[recentSearchesToolStripMenuItem.DropDownItems.Count - 1].Click += LoadRecentlyUsedSearch;
            }

            //enforce the "maxSearches" param
            while (maxSearches < recentSearchesToolStripMenuItem.DropDownItems.Count)
            {
                recentSearchesToolStripMenuItem.DropDownItems.RemoveAt(0);
            }
        }

        private void LoadRecentlyUsedSearch(object sender, EventArgs e)
        {
            //if a search is already running, prompt to cancel.
            Boolean SearchInProgress = SearchIsActive();
            if (SearchInProgress)
            {
                if(DialogResult.Yes == MessageBox.Show("Cancel the current search?", "Cancel", MessageBoxButtons.YesNo))
                {
                    CancelSearch();
                    SearchInProgress = false;
                }
                else
                {
                    SearchInProgress = true;
                }
            }

            if (!SearchInProgress)
            {
                string s_NotFoundMsg = "File not found." + Environment.NewLine + "Remove from the list?"; 
                string filename = (sender as ToolStripMenuItem).Text;
                int idxOfFileName = -1;
                for (int i = 0; i < recentSearchesToolStripMenuItem.DropDownItems.Count; ++i)
                {
                    if (filename == recentSearchesToolStripMenuItem.DropDownItems[i].Text)
                    {
                        idxOfFileName = i;
                    }
                }

                if (System.IO.File.Exists(filename))
                {
                    LoadSavedSearch(filename);
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show(s_NotFoundMsg, "Error", MessageBoxButtons.YesNo))
                    {
                        if (-1 < idxOfFileName)
                        {
                            recentSearchesToolStripMenuItem.DropDownItems.RemoveAt(idxOfFileName);
                        }
                    }
                }
            }
        }

        private void cboSearchFolders_DropDown(object sender, EventArgs e)
        {
            Util.RemoveEmptyEntriesFromCombo(ref cboSearchFolders);
        }

        private void cbOnlyFiles_CheckedChanged(object sender, EventArgs e)
        {
            SetOnlyFilesVisibilites();
        }

        private void SetOnlyFilesVisibilites()
        {
            if (rtbSearchTerms.Enabled == cbOnlyFiles.Checked)
            {
                rtbSearchTerms.Enabled = !cbOnlyFiles.Checked;
                rtbExcludes.Enabled = !cbOnlyFiles.Checked;
            }
        }

        private void txbDynamicResize(object sender, EventArgs e)
        {
            int c_MinSize = 100;
            int c_MaxSize = 300;
            TextBox tmp = (sender as TextBox);

            (sender as TextBox).Width = Math.Max(Math.Min(c_MaxSize, Util.GetPixelWidthOfFormattedText(tmp.Text,tmp.Font,tmp.Width)), c_MinSize);
        }

        private void lblCrippled_Click(object sender, EventArgs e)
        {
            BegForMoney();
            SetPerformanceCrippling(!regInfo.PaidFor());
        }

        private void btnGenKey_Click(object sender, EventArgs e)
        {
            Registration r = new Registration();
            string key = r.NewKey();
            txbKeyToValidate.Text = key;
        }

        private void btnCheckKey_Click(object sender, EventArgs e)
        {
            Registration r = new Registration();
            MessageBox.Show(r.ValidKey(txbKeyToValidate.Text).ToString());
        }

        private void cboSearchFolders_TextChanged(object sender, EventArgs e)
        {
            HighlightInvalidFolder();
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            ForceStandardFontOnRichTextBox((sender as RichTextBox));
        }

        private void ForceStandardFontOnRichTextBox(object rtb)
        {
            //they might copy & paste something from a formatted editor
            //(i.e., a rich text email or ms word)
            //in that case, strip away their formatting and make it look
            //consistent with the rest of the UI in our little world
            int prevSel = (rtb as RichTextBox).SelectionStart;
            (rtb as RichTextBox).SelectAll();
            (rtb as RichTextBox).SelectionFont = cboSearchFolders.Font;
            (rtb as RichTextBox).SelectionColor = cboSearchFolders.ForeColor;
            (rtb as RichTextBox).Select(prevSel, 0);
        }

        private void rtbSearchTerms_Leave(object sender, EventArgs e)
        {
            //it does not make sense to search for "nothing"
            rtbSearchTerms.Lines = RemoveEmptyEntriesFromStringArray(rtbSearchTerms.Lines);
        }

        private void rtbExcludes_Leave(object sender, EventArgs e)
        {
            //it does not make sense to exclude "nothing"
            rtbExcludes.Lines = RemoveEmptyEntriesFromStringArray(rtbExcludes.Lines);
        }

        private string[] RemoveEmptyEntriesFromStringArray(string[] strarry)
        {
            //copy the input array into a new one, skipping empty entries
            try
            {
                int reslen = 0;
                string[] result = new string[reslen];
                foreach (string s in strarry)
                {
                    if (0 < s.Length)
                    {
                        Array.Resize(ref result, ++reslen);
                        result[reslen - 1] = s;
                    }
                }
                return result;
            }
            catch
            {
                //whoopsy.... just return the input array...
                return strarry;
            }
        }

        private void splitCont_MouseDown(object sender, MouseEventArgs e)
        {
            // This disables the normal move behavior
            ((SplitContainer)sender).IsSplitterFixed = true;
        }

        private void splitCont_MouseUp(object sender, MouseEventArgs e)
        {
            // This allows the splitter to be moved normally again
            ((SplitContainer)sender).IsSplitterFixed = false;
        }

        private void splitCont_MouseMove(object sender, MouseEventArgs e)
        {
            // Check to make sure the splitter won't be updated by the
            // normal move behavior also
            if (((SplitContainer)sender).IsSplitterFixed)
            {
                // Make sure that the button used to move the splitter
                // is the left mouse button
                if (e.Button.Equals(MouseButtons.Left))
                {
                    // Checks to see if the splitter is aligned Vertically
                    if (((SplitContainer)sender).Orientation.Equals(Orientation.Vertical))
                    {
                        // Only move the splitter if the mouse is within
                        // the appropriate bounds
                        if (e.X > 0 && e.X < ((SplitContainer)sender).Width)
                        {
                            // Move the splitter & force a visual refresh
                            ((SplitContainer)sender).SplitterDistance = e.X;
                            ((SplitContainer)sender).Refresh();
                        }
                    }
                    // If it isn't aligned vertically then it must be
                    // horizontal
                    else
                    {
                        // Only move the splitter if the mouse is within
                        // the appropriate bounds
                        if (e.Y > 0 && e.Y < ((SplitContainer)sender).Height)
                        {
                            // Move the splitter & force a visual refresh
                            ((SplitContainer)sender).SplitterDistance = e.Y;
                            ((SplitContainer)sender).Refresh();
                        }
                    }
                }
                // If a button other than left is pressed or no button
                // at all
                else
                {
                    // This allows the splitter to be moved normally again
                    ((SplitContainer)sender).IsSplitterFixed = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registration rs = new Registration();
            for (int i = 0; i < 100000; ++i)
            {
                string testkey = rs.NewKey();
                if (!rs.ValidKey(testkey))
                {
                    MessageBox.Show("Failed on '" + testkey + "'");
                }
            }
            MessageBox.Show("success");
        }

        private void SetSearchTermsWidth()
        {
            rtbSearchTerms.Width = SizeOfSearchTermsBox(ref rtbSearchTerms, lblSearchTerms.Width, tpgBasic.Width - rtbSearchTerms.Left, c_buffer);
        }

        private void rtbSearchTerms_KeyPress(object sender, KeyPressEventArgs e)
        {
            SetSearchTermsWidth();
        }
    }
}