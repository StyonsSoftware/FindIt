using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Findit
{
    public partial class frmMain : Form
    {
        GrepTool.Grepper gp;
        Thread searchThread;
        GrepTool.Grepper.UserParameters uparms;
        private int LastReadResult = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //CueTextHandler.SetCueText(txbSearchFor, "Search term");
            CueTextHandler.SetCueText(cboSearchFolders, "Folder to search");
            splitCont.Panel2Collapsed = true;
            ManuallyArrangeOutputArea();
            LoadPrefsFromRegistry();
            HighlightInvalidFolder();
            cboSearchFolders.Select();
            openFileInCustomEditorToolStripMenuItem.ToolTipText = "Opens the selected file in an editor of your choice" +
                Environment.NewLine + "To change the custom editor, use the Tools->Options menu";
            openEnclosingFolderToolStripMenuItem.ToolTipText = "test" + Environment.NewLine + "me";
        }

        private void ManuallyArrangeOutputArea()
        {
            splitCont.Left = 0;
            splitCont.Width = panMid.Width;
            splitCont.Height = panMid.Height - 150;
            splitCont.SplitterDistance = panMid.Width / 2;
        }

        private void SavePrefsToRegistry()
        {
            GUIPreferences gp = new GUIPreferences();
            gp.FileTypeFilter = txbFileType.Text;
            gp.FileExcludeFilter = txbFileExclude.Text;
            gp.IncludeLineNosInOutput = cbLineNos.Checked;
            gp.SearchFolder = cboSearchFolders.Text;
            gp.IncludeLineNosInOutput = cbLineNos.Checked;
            gp.SearchSubfolders = cbIncludeSubfolders.Checked;
            gp.CaseSensitive = cbCaseSensitive.Checked;
            gp.IncludePerfStats = cbPerfStats.Checked;
            gp.SearchTerms = rtbSearchTerms.Lines;
            gp.ExcludeTerms = rtbExcludes.Lines;
            gp.RecentSearchFolders = ComboToStrArry(ref cboSearchFolders, 10);
            if((gp.RecentSearchFolders.Length == 1) && (gp.RecentSearchFolders[0].Length == 0))
            {
                gp.RecentSearchFolders = null;
            }
            gp.SaveToRegistry();
        }

        private string[] ComboToStrArry(ref ComboBox cbo, int cutoff)
        {
            //transfer the combo box items into a string array
            string[] result = { "" };

            while (cboSearchFolders.Items.Count > cutoff)
            {
                cboSearchFolders.Items.RemoveAt(0);
            }

            int nonblankitemcount = 0;
            foreach (string s in cboSearchFolders.Items)
            {
                if (0 < s.Length)
                {
                    nonblankitemcount++;
                }
            }

            int n = 0;
            Array.Resize(ref result, nonblankitemcount);
            for (int i = 0; i < cboSearchFolders.Items.Count; ++i)
            {
                string currentitem = cboSearchFolders.Items[i].ToString();
                if (0 < currentitem.Length)
                {
                    result[n] = currentitem;
                    n++;
                }
            }
            return result;
        }

        private void StrArryToComboItems(ref ComboBox cbo, string[] strarry)
        {
            cbo.Items.Clear();
            foreach (string s in strarry)
            {
                cbo.Items.Add(s);
            }
        }

        private void LoadPrefsFromRegistry()
        {

            GUIPreferences gp = new GUIPreferences();
            gp.LoadFromRegistry();
            cboSearchFolders.Text = gp.SearchFolder;
            txbFileType.Text = gp.FileTypeFilter;
            txbFileExclude.Text = gp.FileExcludeFilter;
            cbLineNos.Checked = gp.IncludeLineNosInOutput;
            cbIncludeSubfolders.Checked = gp.SearchSubfolders;
            cbCaseSensitive.Checked = gp.CaseSensitive;
            cbPerfStats.Checked = gp.IncludePerfStats;
            rtbSearchTerms.Lines = gp.SearchTerms;
            rtbExcludes.Lines = gp.ExcludeTerms;
            StrArryToComboItems(ref cboSearchFolders, gp.RecentSearchFolders);
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
            ConductSearch();
            if(-1 == cboSearchFolders.Items.IndexOf(cboSearchFolders.Text))
            {
                cboSearchFolders.Items.Add(cboSearchFolders.Text);
            }
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
            if (LastReadResult < gp.SearchResultCount)
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
            }
            else
            {
                btnSearch.Enabled = true;
                btnSearch.Text = "Search";
                runSearchToolStripMenuItem.Visible = true;
                cancelSearchToolStripMenuItem.Visible = false;
                lblProgress.Text = "Search complete!";
                
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
                writeoutput("Elapsed seconds : " + gp.PerformanceStats.ElapsedSeconds.ToString());
                writeoutput("Files searched  : " + gp.PerformanceStats.FilesSearched.ToString());
                writeoutput("Lines searched  : " + gp.PerformanceStats.LinesSearched.ToString());
                writeoutput("Files per second: " + fps.ToString());
                writeoutput("Lines per second: " + lps.ToString());
                writeoutput("Matches         : " + gp.PerformanceStats.Matches.ToString());
                writeoutput("Skipped (binary): " + gp.PerformanceStats.Skipped.ToString());
                writeoutput("Errors          : " + gp.PerformanceStats.Errors.ToString());
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
            SavePrefsToRegistry();
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
            SavePrefsToRegistry();
            e.Cancel = !QuitApplication();            
        }

        private void txbDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            HighlightInvalidFolder();
        }

        private void RefreshExcerptPane(string filename, Int64 linenumber)
        {
            rtbExcerpt.Clear();
            Int64 LastReadLine = 1;
            Int64 StartPoint = linenumber - 10;
            Int64 EndPoint = linenumber + 10;
            System.IO.StreamReader reader = new System.IO.StreamReader(filename);
            string currentLine = "";
            do
            {
                currentLine = reader.ReadLine();
                if ((currentLine != null) && (LastReadLine > StartPoint) && (LastReadLine < EndPoint))
                {
                    rtbExcerpt.AppendText(currentLine + Environment.NewLine);
                }
                LastReadLine++;

                if (LastReadLine > EndPoint)
                {
                    break;
                }
            } while (!reader.EndOfStream);
            foreach (string s in rtbSearchTerms.Lines)
            {
                HighlightWordInRtb(ref rtbExcerpt, s);
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
                    RefreshExcerptPane(selectedfname, CenterOnThisLine);
                }
            }
            catch
            {
                //do nothing
            }
        }

        private void HighlightWordInRtb(ref RichTextBox rtb, string WordToHighlight)
        {
            int i = 0;
            while (i != -1)
            {
                i = rtb.Text.IndexOf(WordToHighlight, i, StringComparison.CurrentCultureIgnoreCase);
                if (i != -1)
                {
                    rtb.SelectionStart = i;
                    rtb.SelectionLength = WordToHighlight.Length;
                    rtb.SelectionColor = Color.Red;
                    i = i + WordToHighlight.Length;
                }
            }
        }

        private void OpenFile(string fname, string customexe)
        {
            if (System.IO.File.Exists(fname) || System.IO.Directory.Exists(fname))
            {
                if (System.IO.File.Exists(customexe))
                {
                    //start using the custom exe they asked for
                    Process ntpdplpl = new Process();
                    ntpdplpl.StartInfo.FileName = customexe;
                    ntpdplpl.StartInfo.Arguments = fname;
                    ntpdplpl.Start();
                }
                else
                {
                    //let the system decide what is the best editor
                    System.Diagnostics.Process.Start(fname);
                }
            }
        }

        private void lbResults_DoubleClick(object sender, EventArgs e)
        {
            OpenFileInCustomEditor(SelectedResultsFile());
        }

        private void btnTogglePreview_Click(object sender, EventArgs e)
        {
            splitCont.Panel2Collapsed = !splitCont.Panel2Collapsed;
        }

        private void frmMain_ClientSizeChanged(object sender, EventArgs e)
        {
            ManuallyArrangeOutputArea();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearOptions();
        }

        private void ClearOptions()
        {
            cboSearchFolders.Text = @"C:\";
            rtbSearchTerms.Clear();
            rtbExcludes.Clear();
            rtbExcerpt.Clear();
            lbResults.Items.Clear();
            txbFileType.Text = "*.*";
            txbFileExclude.Clear();
            cbCaseSensitive.Checked = false;
            cbIncludeSubfolders.Checked = true;
            cbLineNos.Checked = false;
            cbPerfStats.Checked = false;
        }

        private void txbDirectory_TextChanged(object sender, EventArgs e)
        {
            HighlightInvalidFolder();
        }

        private void runSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConductSearch();
        }

        private void cancelSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelSearch();
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
                OpenFile(lbResults.Items[lbResults.SelectedIndex].ToString().Trim(),"");
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
                OpenFileInCustomEditor(SelectedResultsFile());
            }
            else
            {
                MessageBox.Show("Select a row first!");
            }
        }

        private void OpenFileInCustomEditor(string filename)
        {
            OpenFile(filename,CustomEditorExeName());
        }

        private string CustomEditorExeName()
        {
            GUIPreferences gp = new GUIPreferences();
            return gp.CustomEditorExe;
        }

        private void openEnclosingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (0 < SelectedResultsFile().Length)
            {
                OpenEnclosingFolder(SelectedResultsFile());
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

        private void OpenEnclosingFolder(string fullfilename)
        {
            if (System.IO.File.Exists(fullfilename))
            {
                OpenFile(System.IO.Directory.GetParent(fullfilename).ToString(), "");
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

    }
}
