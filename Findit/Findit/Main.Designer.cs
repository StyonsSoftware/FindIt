namespace Findit
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.rtbSearchTerms = new System.Windows.Forms.RichTextBox();
            this.txbFileExclude = new System.Windows.Forms.TextBox();
            this.cbIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.cbPerfStats = new System.Windows.Forms.CheckBox();
            this.cbCaseSensitive = new System.Windows.Forms.CheckBox();
            this.cbLineNos = new System.Windows.Forms.CheckBox();
            this.rtbExcludes = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txbFileType = new System.Windows.Forms.TextBox();
            this.panTop = new System.Windows.Forms.Panel();
            this.grbBasicOptions = new System.Windows.Forms.GroupBox();
            this.tabctlSearchOptions = new System.Windows.Forms.TabControl();
            this.tpgBasic = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.txbKeyToValidate = new System.Windows.Forms.TextBox();
            this.btnCheckKey = new System.Windows.Forms.Button();
            this.btnGenKey = new System.Windows.Forms.Button();
            this.lblFileType = new System.Windows.Forms.Label();
            this.lblSearchTerms = new System.Windows.Forms.Label();
            this.cboSearchFolders = new System.Windows.Forms.ComboBox();
            this.lblInvalidNotice = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
            this.tpgAdvanced = new System.Windows.Forms.TabPage();
            this.cbIncludeOffice = new System.Windows.Forms.CheckBox();
            this.cbOnlyFiles = new System.Windows.Forms.CheckBox();
            this.lblExcludes = new System.Windows.Forms.Label();
            this.lblFileExclude = new System.Windows.Forms.Label();
            this.panMid = new System.Windows.Forms.Panel();
            this.splitCont = new System.Windows.Forms.SplitContainer();
            this.lbResults = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedRowToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileInDefaultEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEnclosingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileInCustomEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbExcerpt = new System.Windows.Forms.RichTextBox();
            this.panLabel = new System.Windows.Forms.Panel();
            this.lblCrippled = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblSrchRes = new System.Windows.Forms.Label();
            this.panBottom = new System.Windows.Forms.Panel();
            this.timerRefreshGUI = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveThisSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadASavedSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentSearchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panTop.SuspendLayout();
            this.grbBasicOptions.SuspendLayout();
            this.tabctlSearchOptions.SuspendLayout();
            this.tpgBasic.SuspendLayout();
            this.tpgAdvanced.SuspendLayout();
            this.panMid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCont)).BeginInit();
            this.splitCont.Panel1.SuspendLayout();
            this.splitCont.Panel2.SuspendLayout();
            this.splitCont.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panLabel.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(0, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(171, 81);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel a running search (or push Ctrl+END)");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Image = global::Findit.Properties.Resources.Search;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(773, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(173, 81);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "&Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnSearch, "Search using the given criteria (or just push F5)");
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTogglePreview.Image = ((System.Drawing.Image)(resources.GetObject("btnTogglePreview.Image")));
            this.btnTogglePreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTogglePreview.Location = new System.Drawing.Point(893, 0);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(53, 39);
            this.btnTogglePreview.TabIndex = 1;
            this.btnTogglePreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnTogglePreview, "Toggle preview");
            this.btnTogglePreview.UseVisualStyleBackColor = true;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(525, 35);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(37, 26);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.toolTip1.SetToolTip(this.btnBrowse, "Browse for a folder in a separate window");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // rtbSearchTerms
            // 
            this.rtbSearchTerms.AcceptsTab = true;
            this.rtbSearchTerms.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSearchTerms.Location = new System.Drawing.Point(614, 35);
            this.rtbSearchTerms.Margin = new System.Windows.Forms.Padding(4);
            this.rtbSearchTerms.Name = "rtbSearchTerms";
            this.rtbSearchTerms.Size = new System.Drawing.Size(309, 132);
            this.rtbSearchTerms.TabIndex = 0;
            this.rtbSearchTerms.Text = "";
            this.toolTip1.SetToolTip(this.rtbSearchTerms, "Separate multiple terms with the ENTER key");
            this.rtbSearchTerms.WordWrap = false;
            this.rtbSearchTerms.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            this.rtbSearchTerms.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbSearchTerms_KeyPress);
            this.rtbSearchTerms.Leave += new System.EventHandler(this.rtbSearchTerms_Leave);
            // 
            // txbFileExclude
            // 
            this.txbFileExclude.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbFileExclude.Location = new System.Drawing.Point(10, 25);
            this.txbFileExclude.Margin = new System.Windows.Forms.Padding(4);
            this.txbFileExclude.Name = "txbFileExclude";
            this.txbFileExclude.Size = new System.Drawing.Size(113, 26);
            this.txbFileExclude.TabIndex = 24;
            this.toolTip1.SetToolTip(this.txbFileExclude, "Exclude files that match this filter (default = blank)");
            this.txbFileExclude.TextChanged += new System.EventHandler(this.txbDynamicResize);
            // 
            // cbIncludeSubfolders
            // 
            this.cbIncludeSubfolders.AutoSize = true;
            this.cbIncludeSubfolders.Checked = true;
            this.cbIncludeSubfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeSubfolders.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIncludeSubfolders.Location = new System.Drawing.Point(498, 118);
            this.cbIncludeSubfolders.Name = "cbIncludeSubfolders";
            this.cbIncludeSubfolders.Size = new System.Drawing.Size(142, 22);
            this.cbIncludeSubfolders.TabIndex = 22;
            this.cbIncludeSubfolders.Text = "Include subfolders";
            this.toolTip1.SetToolTip(this.cbIncludeSubfolders, "Uncheck to only search within the exact folder you specified.\r\nLeave checked to a" +
        "lso include all subfolders that fall under it.");
            this.cbIncludeSubfolders.UseVisualStyleBackColor = true;
            // 
            // cbPerfStats
            // 
            this.cbPerfStats.AutoSize = true;
            this.cbPerfStats.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPerfStats.Location = new System.Drawing.Point(499, 89);
            this.cbPerfStats.Margin = new System.Windows.Forms.Padding(4);
            this.cbPerfStats.Name = "cbPerfStats";
            this.cbPerfStats.Size = new System.Drawing.Size(271, 22);
            this.cbPerfStats.TabIndex = 21;
            this.cbPerfStats.Text = "Include performance statistics in output";
            this.toolTip1.SetToolTip(this.cbPerfStats, "Adds some performance statistics to the end of the output");
            this.cbPerfStats.UseVisualStyleBackColor = true;
            // 
            // cbCaseSensitive
            // 
            this.cbCaseSensitive.AutoSize = true;
            this.cbCaseSensitive.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCaseSensitive.Location = new System.Drawing.Point(499, 59);
            this.cbCaseSensitive.Margin = new System.Windows.Forms.Padding(4);
            this.cbCaseSensitive.Name = "cbCaseSensitive";
            this.cbCaseSensitive.Size = new System.Drawing.Size(114, 22);
            this.cbCaseSensitive.TabIndex = 20;
            this.cbCaseSensitive.Text = "Case sensitive";
            this.toolTip1.SetToolTip(this.cbCaseSensitive, "Check to make the search sensitive to upper and lowercase letters");
            this.cbCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cbLineNos
            // 
            this.cbLineNos.AutoSize = true;
            this.cbLineNos.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLineNos.Location = new System.Drawing.Point(499, 29);
            this.cbLineNos.Margin = new System.Windows.Forms.Padding(4);
            this.cbLineNos.Name = "cbLineNos";
            this.cbLineNos.Size = new System.Drawing.Size(218, 22);
            this.cbLineNos.TabIndex = 19;
            this.cbLineNos.Text = "Include line numbers in output";
            this.toolTip1.SetToolTip(this.cbLineNos, "Adds \"@ ##\" to the end of each output line, indicating the line # within the file" +
        " that your match was found");
            this.cbLineNos.UseVisualStyleBackColor = true;
            // 
            // rtbExcludes
            // 
            this.rtbExcludes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExcludes.Location = new System.Drawing.Point(198, 25);
            this.rtbExcludes.Margin = new System.Windows.Forms.Padding(4);
            this.rtbExcludes.Name = "rtbExcludes";
            this.rtbExcludes.Size = new System.Drawing.Size(293, 143);
            this.rtbExcludes.TabIndex = 17;
            this.rtbExcludes.Text = "";
            this.toolTip1.SetToolTip(this.rtbExcludes, "Separate multiple terms with the ENTER key");
            this.rtbExcludes.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            this.rtbExcludes.Leave += new System.EventHandler(this.rtbExcludes_Leave);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(6, 137);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(189, 30);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Reset filters to default";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnClear, "Clears the options, resetting them to the default values");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txbFileType
            // 
            this.txbFileType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbFileType.Location = new System.Drawing.Point(6, 104);
            this.txbFileType.Margin = new System.Windows.Forms.Padding(4);
            this.txbFileType.Name = "txbFileType";
            this.txbFileType.Size = new System.Drawing.Size(69, 26);
            this.txbFileType.TabIndex = 19;
            this.txbFileType.Text = "*.*";
            this.toolTip1.SetToolTip(this.txbFileType, "Only search within files that match this file type (ex: *.txt)");
            this.txbFileType.TextChanged += new System.EventHandler(this.txbDynamicResize);
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.grbBasicOptions);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 24);
            this.panTop.Margin = new System.Windows.Forms.Padding(4);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(946, 239);
            this.panTop.TabIndex = 0;
            // 
            // grbBasicOptions
            // 
            this.grbBasicOptions.Controls.Add(this.tabctlSearchOptions);
            this.grbBasicOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbBasicOptions.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbBasicOptions.Location = new System.Drawing.Point(0, 0);
            this.grbBasicOptions.Margin = new System.Windows.Forms.Padding(4);
            this.grbBasicOptions.Name = "grbBasicOptions";
            this.grbBasicOptions.Padding = new System.Windows.Forms.Padding(4);
            this.grbBasicOptions.Size = new System.Drawing.Size(946, 239);
            this.grbBasicOptions.TabIndex = 0;
            this.grbBasicOptions.TabStop = false;
            this.grbBasicOptions.Text = "Search options";
            // 
            // tabctlSearchOptions
            // 
            this.tabctlSearchOptions.Controls.Add(this.tpgBasic);
            this.tabctlSearchOptions.Controls.Add(this.tpgAdvanced);
            this.tabctlSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabctlSearchOptions.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabctlSearchOptions.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabctlSearchOptions.Location = new System.Drawing.Point(4, 23);
            this.tabctlSearchOptions.Name = "tabctlSearchOptions";
            this.tabctlSearchOptions.SelectedIndex = 0;
            this.tabctlSearchOptions.Size = new System.Drawing.Size(938, 212);
            this.tabctlSearchOptions.TabIndex = 0;
            this.tabctlSearchOptions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabctlSearchOptions_DrawItem);
            // 
            // tpgBasic
            // 
            this.tpgBasic.BackColor = System.Drawing.SystemColors.Control;
            this.tpgBasic.Controls.Add(this.button1);
            this.tpgBasic.Controls.Add(this.txbKeyToValidate);
            this.tpgBasic.Controls.Add(this.btnCheckKey);
            this.tpgBasic.Controls.Add(this.btnGenKey);
            this.tpgBasic.Controls.Add(this.lblFileType);
            this.tpgBasic.Controls.Add(this.txbFileType);
            this.tpgBasic.Controls.Add(this.btnClear);
            this.tpgBasic.Controls.Add(this.lblSearchTerms);
            this.tpgBasic.Controls.Add(this.rtbSearchTerms);
            this.tpgBasic.Controls.Add(this.cboSearchFolders);
            this.tpgBasic.Controls.Add(this.lblInvalidNotice);
            this.tpgBasic.Controls.Add(this.lblFolder);
            this.tpgBasic.Controls.Add(this.btnBrowse);
            this.tpgBasic.Location = new System.Drawing.Point(4, 27);
            this.tpgBasic.Name = "tpgBasic";
            this.tpgBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tpgBasic.Size = new System.Drawing.Size(930, 181);
            this.tpgBasic.TabIndex = 0;
            this.tpgBasic.Text = "Basic options";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(262, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 33);
            this.button1.TabIndex = 24;
            this.button1.Text = "Full key test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbKeyToValidate
            // 
            this.txbKeyToValidate.Location = new System.Drawing.Point(163, 82);
            this.txbKeyToValidate.Name = "txbKeyToValidate";
            this.txbKeyToValidate.Size = new System.Drawing.Size(260, 26);
            this.txbKeyToValidate.TabIndex = 23;
            this.txbKeyToValidate.Visible = false;
            // 
            // btnCheckKey
            // 
            this.btnCheckKey.Location = new System.Drawing.Point(429, 118);
            this.btnCheckKey.Name = "btnCheckKey";
            this.btnCheckKey.Size = new System.Drawing.Size(133, 42);
            this.btnCheckKey.TabIndex = 22;
            this.btnCheckKey.Text = "Validate a key";
            this.btnCheckKey.UseVisualStyleBackColor = true;
            this.btnCheckKey.Visible = false;
            this.btnCheckKey.Click += new System.EventHandler(this.btnCheckKey_Click);
            // 
            // btnGenKey
            // 
            this.btnGenKey.Location = new System.Drawing.Point(429, 70);
            this.btnGenKey.Name = "btnGenKey";
            this.btnGenKey.Size = new System.Drawing.Size(133, 42);
            this.btnGenKey.TabIndex = 21;
            this.btnGenKey.Text = "Generate a key";
            this.btnGenKey.UseVisualStyleBackColor = true;
            this.btnGenKey.Visible = false;
            this.btnGenKey.Click += new System.EventHandler(this.btnGenKey_Click);
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileType.Location = new System.Drawing.Point(3, 82);
            this.lblFileType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(102, 18);
            this.lblFileType.TabIndex = 20;
            this.lblFileType.Text = "File name filter";
            // 
            // lblSearchTerms
            // 
            this.lblSearchTerms.AutoSize = true;
            this.lblSearchTerms.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchTerms.Location = new System.Drawing.Point(611, 14);
            this.lblSearchTerms.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchTerms.Name = "lblSearchTerms";
            this.lblSearchTerms.Size = new System.Drawing.Size(261, 18);
            this.lblSearchTerms.TabIndex = 18;
            this.lblSearchTerms.Text = "For files that contain *all* of these words";
            // 
            // cboSearchFolders
            // 
            this.cboSearchFolders.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSearchFolders.FormattingEnabled = true;
            this.cboSearchFolders.Location = new System.Drawing.Point(6, 35);
            this.cboSearchFolders.MaxDropDownItems = 20;
            this.cboSearchFolders.Name = "cboSearchFolders";
            this.cboSearchFolders.Size = new System.Drawing.Size(512, 26);
            this.cboSearchFolders.TabIndex = 1;
            this.cboSearchFolders.DropDown += new System.EventHandler(this.cboSearchFolders_DropDown);
            this.cboSearchFolders.TextChanged += new System.EventHandler(this.cboSearchFolders_TextChanged);
            // 
            // lblInvalidNotice
            // 
            this.lblInvalidNotice.AutoSize = true;
            this.lblInvalidNotice.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvalidNotice.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidNotice.Location = new System.Drawing.Point(386, 14);
            this.lblInvalidNotice.Name = "lblInvalidNotice";
            this.lblInvalidNotice.Size = new System.Drawing.Size(132, 18);
            this.lblInvalidNotice.TabIndex = 16;
            this.lblInvalidNotice.Text = "This folder is invalid";
            this.lblInvalidNotice.Visible = false;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolder.Location = new System.Drawing.Point(3, 13);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(120, 18);
            this.lblFolder.TabIndex = 15;
            this.lblFolder.Text = "Search this folder:";
            // 
            // tpgAdvanced
            // 
            this.tpgAdvanced.BackColor = System.Drawing.SystemColors.Control;
            this.tpgAdvanced.Controls.Add(this.cbIncludeOffice);
            this.tpgAdvanced.Controls.Add(this.cbOnlyFiles);
            this.tpgAdvanced.Controls.Add(this.lblExcludes);
            this.tpgAdvanced.Controls.Add(this.txbFileExclude);
            this.tpgAdvanced.Controls.Add(this.lblFileExclude);
            this.tpgAdvanced.Controls.Add(this.cbIncludeSubfolders);
            this.tpgAdvanced.Controls.Add(this.cbPerfStats);
            this.tpgAdvanced.Controls.Add(this.cbCaseSensitive);
            this.tpgAdvanced.Controls.Add(this.cbLineNos);
            this.tpgAdvanced.Controls.Add(this.rtbExcludes);
            this.tpgAdvanced.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpgAdvanced.Location = new System.Drawing.Point(4, 27);
            this.tpgAdvanced.Name = "tpgAdvanced";
            this.tpgAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tpgAdvanced.Size = new System.Drawing.Size(930, 181);
            this.tpgAdvanced.TabIndex = 1;
            this.tpgAdvanced.Text = "Advanced options";
            // 
            // cbIncludeOffice
            // 
            this.cbIncludeOffice.AutoSize = true;
            this.cbIncludeOffice.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIncludeOffice.Location = new System.Drawing.Point(498, 146);
            this.cbIncludeOffice.Name = "cbIncludeOffice";
            this.cbIncludeOffice.Size = new System.Drawing.Size(206, 22);
            this.cbIncludeOffice.TabIndex = 27;
            this.cbIncludeOffice.Text = "Include MS Office documents";
            this.cbIncludeOffice.UseVisualStyleBackColor = true;
            // 
            // cbOnlyFiles
            // 
            this.cbOnlyFiles.AutoSize = true;
            this.cbOnlyFiles.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOnlyFiles.Location = new System.Drawing.Point(10, 58);
            this.cbOnlyFiles.Name = "cbOnlyFiles";
            this.cbOnlyFiles.Size = new System.Drawing.Size(187, 22);
            this.cbOnlyFiles.TabIndex = 26;
            this.cbOnlyFiles.Text = "Only search for file names";
            this.cbOnlyFiles.UseVisualStyleBackColor = true;
            // 
            // lblExcludes
            // 
            this.lblExcludes.AutoSize = true;
            this.lblExcludes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcludes.Location = new System.Drawing.Point(195, 3);
            this.lblExcludes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExcludes.Name = "lblExcludes";
            this.lblExcludes.Size = new System.Drawing.Size(296, 18);
            this.lblExcludes.TabIndex = 25;
            this.lblExcludes.Text = "*Exclude* files that contain any of these words";
            // 
            // lblFileExclude
            // 
            this.lblFileExclude.AutoSize = true;
            this.lblFileExclude.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileExclude.Location = new System.Drawing.Point(7, 3);
            this.lblFileExclude.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileExclude.Name = "lblFileExclude";
            this.lblFileExclude.Size = new System.Drawing.Size(116, 18);
            this.lblFileExclude.TabIndex = 23;
            this.lblFileExclude.Text = "File exclude filter";
            // 
            // panMid
            // 
            this.panMid.Controls.Add(this.splitCont);
            this.panMid.Controls.Add(this.panLabel);
            this.panMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMid.Location = new System.Drawing.Point(0, 263);
            this.panMid.Margin = new System.Windows.Forms.Padding(4);
            this.panMid.Name = "panMid";
            this.panMid.Size = new System.Drawing.Size(946, 315);
            this.panMid.TabIndex = 0;
            // 
            // splitCont
            // 
            this.splitCont.Location = new System.Drawing.Point(404, 67);
            this.splitCont.Name = "splitCont";
            // 
            // splitCont.Panel1
            // 
            this.splitCont.Panel1.Controls.Add(this.lbResults);
            // 
            // splitCont.Panel2
            // 
            this.splitCont.Panel2.Controls.Add(this.rtbExcerpt);
            this.splitCont.Size = new System.Drawing.Size(490, 100);
            this.splitCont.SplitterDistance = 273;
            this.splitCont.SplitterWidth = 10;
            this.splitCont.TabIndex = 4;
            this.splitCont.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitCont_MouseDown);
            this.splitCont.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitCont_MouseMove);
            this.splitCont.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitCont_MouseUp);
            // 
            // lbResults
            // 
            this.lbResults.ContextMenuStrip = this.contextMenuStrip1;
            this.lbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbResults.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResults.FormattingEnabled = true;
            this.lbResults.HorizontalScrollbar = true;
            this.lbResults.ItemHeight = 18;
            this.lbResults.Location = new System.Drawing.Point(0, 0);
            this.lbResults.Name = "lbResults";
            this.lbResults.Size = new System.Drawing.Size(273, 100);
            this.lbResults.TabIndex = 3;
            this.lbResults.SelectedIndexChanged += new System.EventHandler(this.lbResults_SelectedIndexChanged);
            this.lbResults.DoubleClick += new System.EventHandler(this.lbResults_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem,
            this.copySelectedRowToClipboardToolStripMenuItem,
            this.openFileInDefaultEditorToolStripMenuItem,
            this.openEnclosingFolderToolStripMenuItem,
            this.openFileInCustomEditorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(223, 114);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToClipboardToolStripMenuItem.Image")));
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy ALL rows to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // copySelectedRowToClipboardToolStripMenuItem
            // 
            this.copySelectedRowToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copySelectedRowToClipboardToolStripMenuItem.Image")));
            this.copySelectedRowToClipboardToolStripMenuItem.Name = "copySelectedRowToClipboardToolStripMenuItem";
            this.copySelectedRowToClipboardToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.copySelectedRowToClipboardToolStripMenuItem.Text = "Copy selected row to clipboard";
            this.copySelectedRowToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySelectedRowToClipboardToolStripMenuItem_Click);
            // 
            // openFileInDefaultEditorToolStripMenuItem
            // 
            this.openFileInDefaultEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFileInDefaultEditorToolStripMenuItem.Image")));
            this.openFileInDefaultEditorToolStripMenuItem.Name = "openFileInDefaultEditorToolStripMenuItem";
            this.openFileInDefaultEditorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.openFileInDefaultEditorToolStripMenuItem.Text = "Open file in default editor";
            this.openFileInDefaultEditorToolStripMenuItem.Click += new System.EventHandler(this.openFileInDefaultEditorToolStripMenuItem_Click);
            // 
            // openEnclosingFolderToolStripMenuItem
            // 
            this.openEnclosingFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openEnclosingFolderToolStripMenuItem.Image")));
            this.openEnclosingFolderToolStripMenuItem.Name = "openEnclosingFolderToolStripMenuItem";
            this.openEnclosingFolderToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.openEnclosingFolderToolStripMenuItem.Text = "Open enclosing folder";
            this.openEnclosingFolderToolStripMenuItem.ToolTipText = "Opens the folder that contains this file";
            this.openEnclosingFolderToolStripMenuItem.Click += new System.EventHandler(this.openEnclosingFolderToolStripMenuItem_Click);
            // 
            // openFileInCustomEditorToolStripMenuItem
            // 
            this.openFileInCustomEditorToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFileInCustomEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFileInCustomEditorToolStripMenuItem.Image")));
            this.openFileInCustomEditorToolStripMenuItem.Name = "openFileInCustomEditorToolStripMenuItem";
            this.openFileInCustomEditorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.openFileInCustomEditorToolStripMenuItem.Text = "Open file in custom editor";
            this.openFileInCustomEditorToolStripMenuItem.Click += new System.EventHandler(this.openFileInCustomEditorToolStripMenuItem_Click);
            // 
            // rtbExcerpt
            // 
            this.rtbExcerpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbExcerpt.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExcerpt.Location = new System.Drawing.Point(0, 0);
            this.rtbExcerpt.Name = "rtbExcerpt";
            this.rtbExcerpt.Size = new System.Drawing.Size(207, 100);
            this.rtbExcerpt.TabIndex = 4;
            this.rtbExcerpt.Text = "";
            this.rtbExcerpt.WordWrap = false;
            // 
            // panLabel
            // 
            this.panLabel.Controls.Add(this.lblCrippled);
            this.panLabel.Controls.Add(this.lblProgress);
            this.panLabel.Controls.Add(this.btnTogglePreview);
            this.panLabel.Controls.Add(this.lblSrchRes);
            this.panLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panLabel.Location = new System.Drawing.Point(0, 0);
            this.panLabel.Margin = new System.Windows.Forms.Padding(4);
            this.panLabel.Name = "panLabel";
            this.panLabel.Size = new System.Drawing.Size(946, 39);
            this.panLabel.TabIndex = 1;
            // 
            // lblCrippled
            // 
            this.lblCrippled.AutoSize = true;
            this.lblCrippled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCrippled.ForeColor = System.Drawing.Color.Red;
            this.lblCrippled.Location = new System.Drawing.Point(100, 4);
            this.lblCrippled.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCrippled.Name = "lblCrippled";
            this.lblCrippled.Size = new System.Drawing.Size(234, 13);
            this.lblCrippled.TabIndex = 4;
            this.lblCrippled.Text = "Enter a registration key for much faster searches";
            this.lblCrippled.Visible = false;
            this.lblCrippled.Click += new System.EventHandler(this.lblCrippled_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblProgress.Location = new System.Drawing.Point(4, 21);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 3;
            this.lblProgress.Click += new System.EventHandler(this.lblProgress_Click);
            // 
            // lblSrchRes
            // 
            this.lblSrchRes.AutoSize = true;
            this.lblSrchRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSrchRes.Location = new System.Drawing.Point(4, 4);
            this.lblSrchRes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSrchRes.Name = "lblSrchRes";
            this.lblSrchRes.Size = new System.Drawing.Size(88, 13);
            this.lblSrchRes.TabIndex = 0;
            this.lblSrchRes.Text = "Search results";
            // 
            // panBottom
            // 
            this.panBottom.Controls.Add(this.btnCancel);
            this.panBottom.Controls.Add(this.btnSearch);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panBottom.Location = new System.Drawing.Point(0, 497);
            this.panBottom.Margin = new System.Windows.Forms.Padding(4);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new System.Drawing.Size(946, 81);
            this.panBottom.TabIndex = 2;
            // 
            // timerRefreshGUI
            // 
            this.timerRefreshGUI.Interval = 500;
            this.timerRefreshGUI.Tick += new System.EventHandler(this.timerRefreshGUI_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(946, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSearchToolStripMenuItem,
            this.cancelSearchToolStripMenuItem,
            this.saveThisSearchToolStripMenuItem,
            this.loadASavedSearchToolStripMenuItem,
            this.recentSearchesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // runSearchToolStripMenuItem
            // 
            this.runSearchToolStripMenuItem.Name = "runSearchToolStripMenuItem";
            this.runSearchToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runSearchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.runSearchToolStripMenuItem.Text = "&Search";
            this.runSearchToolStripMenuItem.Click += new System.EventHandler(this.runSearchToolStripMenuItem_Click);
            // 
            // cancelSearchToolStripMenuItem
            // 
            this.cancelSearchToolStripMenuItem.Name = "cancelSearchToolStripMenuItem";
            this.cancelSearchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)));
            this.cancelSearchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.cancelSearchToolStripMenuItem.Text = "&Cancel Search";
            this.cancelSearchToolStripMenuItem.Visible = false;
            this.cancelSearchToolStripMenuItem.Click += new System.EventHandler(this.cancelSearchToolStripMenuItem_Click);
            // 
            // saveThisSearchToolStripMenuItem
            // 
            this.saveThisSearchToolStripMenuItem.Name = "saveThisSearchToolStripMenuItem";
            this.saveThisSearchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveThisSearchToolStripMenuItem.Text = "Save this search &as...";
            this.saveThisSearchToolStripMenuItem.Click += new System.EventHandler(this.saveThisSearchToolStripMenuItem_Click);
            // 
            // loadASavedSearchToolStripMenuItem
            // 
            this.loadASavedSearchToolStripMenuItem.Name = "loadASavedSearchToolStripMenuItem";
            this.loadASavedSearchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.loadASavedSearchToolStripMenuItem.Text = "&Load a saved search...";
            this.loadASavedSearchToolStripMenuItem.Click += new System.EventHandler(this.loadASavedSearchToolStripMenuItem_Click);
            // 
            // recentSearchesToolStripMenuItem
            // 
            this.recentSearchesToolStripMenuItem.Name = "recentSearchesToolStripMenuItem";
            this.recentSearchesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.recentSearchesToolStripMenuItem.Text = "&Recent searches";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // oToolStripMenuItem
            // 
            this.oToolStripMenuItem.Name = "oToolStripMenuItem";
            this.oToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.oToolStripMenuItem.Text = "Options...";
            this.oToolStripMenuItem.Click += new System.EventHandler(this.oToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.registerToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.registerToolStripMenuItem.Text = "Register...";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 578);
            this.Controls.Add(this.panBottom);
            this.Controls.Add(this.panMid);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "FindIt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ClientSizeChanged += new System.EventHandler(this.frmMain_ClientSizeChanged);
            this.panTop.ResumeLayout(false);
            this.grbBasicOptions.ResumeLayout(false);
            this.tabctlSearchOptions.ResumeLayout(false);
            this.tpgBasic.ResumeLayout(false);
            this.tpgBasic.PerformLayout();
            this.tpgAdvanced.ResumeLayout(false);
            this.tpgAdvanced.PerformLayout();
            this.panMid.ResumeLayout(false);
            this.splitCont.Panel1.ResumeLayout(false);
            this.splitCont.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCont)).EndInit();
            this.splitCont.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panLabel.ResumeLayout(false);
            this.panLabel.PerformLayout();
            this.panBottom.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.Panel panMid;
        private System.Windows.Forms.GroupBox grbBasicOptions;
        private System.Windows.Forms.Panel panBottom;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panLabel;
        private System.Windows.Forms.Label lblSrchRes;
        private System.Windows.Forms.Timer timerRefreshGUI;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitCont;
        private System.Windows.Forms.ListBox lbResults;
        private System.Windows.Forms.RichTextBox rtbExcerpt;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.ToolStripMenuItem runSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelSearchToolStripMenuItem;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedRowToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileInDefaultEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEnclosingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileInCustomEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oToolStripMenuItem;
        private System.Windows.Forms.TabControl tabctlSearchOptions;
        private System.Windows.Forms.TabPage tpgBasic;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblSearchTerms;
        private System.Windows.Forms.RichTextBox rtbSearchTerms;
        private System.Windows.Forms.ComboBox cboSearchFolders;
        private System.Windows.Forms.Label lblInvalidNotice;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TabPage tpgAdvanced;
        private System.Windows.Forms.Label lblExcludes;
        private System.Windows.Forms.TextBox txbFileExclude;
        private System.Windows.Forms.Label lblFileExclude;
        private System.Windows.Forms.CheckBox cbIncludeSubfolders;
        private System.Windows.Forms.CheckBox cbPerfStats;
        private System.Windows.Forms.CheckBox cbCaseSensitive;
        private System.Windows.Forms.CheckBox cbLineNos;
        private System.Windows.Forms.RichTextBox rtbExcludes;
        private System.Windows.Forms.ToolStripMenuItem saveThisSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadASavedSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentSearchesToolStripMenuItem;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.TextBox txbFileType;
        private System.Windows.Forms.CheckBox cbOnlyFiles;
        private System.Windows.Forms.Label lblCrippled;
        private System.Windows.Forms.Button btnGenKey;
        private System.Windows.Forms.TextBox txbKeyToValidate;
        private System.Windows.Forms.Button btnCheckKey;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbIncludeOffice;

    }
}

