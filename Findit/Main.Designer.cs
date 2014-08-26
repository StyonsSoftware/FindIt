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
            this.cbIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.cbPerfStats = new System.Windows.Forms.CheckBox();
            this.cbCaseSensitive = new System.Windows.Forms.CheckBox();
            this.cbLineNos = new System.Windows.Forms.CheckBox();
            this.rtbExcludes = new System.Windows.Forms.RichTextBox();
            this.txbFileType = new System.Windows.Forms.TextBox();
            this.rtbSearchTerms = new System.Windows.Forms.RichTextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txbFileExclude = new System.Windows.Forms.TextBox();
            this.panTop = new System.Windows.Forms.Panel();
            this.grbBasicOptions = new System.Windows.Forms.GroupBox();
            this.lblFileExclude = new System.Windows.Forms.Label();
            this.cboSearchFolders = new System.Windows.Forms.ComboBox();
            this.lblInvalidNotice = new System.Windows.Forms.Label();
            this.lblFileType = new System.Windows.Forms.Label();
            this.lblExcludes = new System.Windows.Forms.Label();
            this.lblSearchTerms = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
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
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblSrchRes = new System.Windows.Forms.Label();
            this.panBottom = new System.Windows.Forms.Panel();
            this.timerRefreshGUI = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panTop.SuspendLayout();
            this.grbBasicOptions.SuspendLayout();
            this.panMid.SuspendLayout();
            this.splitCont.Panel1.SuspendLayout();
            this.splitCont.Panel2.SuspendLayout();
            this.splitCont.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panLabel.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbIncludeSubfolders
            // 
            this.cbIncludeSubfolders.AutoSize = true;
            this.cbIncludeSubfolders.Checked = true;
            this.cbIncludeSubfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeSubfolders.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIncludeSubfolders.Location = new System.Drawing.Point(583, 201);
            this.cbIncludeSubfolders.Name = "cbIncludeSubfolders";
            this.cbIncludeSubfolders.Size = new System.Drawing.Size(142, 22);
            this.cbIncludeSubfolders.TabIndex = 11;
            this.cbIncludeSubfolders.Text = "Include subfolders";
            this.toolTip1.SetToolTip(this.cbIncludeSubfolders, "Uncheck to only search within the exact folder you specified.\r\nLeave checked to a" +
                    "lso include all subfolders that fall under it.");
            this.cbIncludeSubfolders.UseVisualStyleBackColor = true;
            // 
            // cbPerfStats
            // 
            this.cbPerfStats.AutoSize = true;
            this.cbPerfStats.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPerfStats.Location = new System.Drawing.Point(583, 172);
            this.cbPerfStats.Margin = new System.Windows.Forms.Padding(4);
            this.cbPerfStats.Name = "cbPerfStats";
            this.cbPerfStats.Size = new System.Drawing.Size(271, 22);
            this.cbPerfStats.TabIndex = 10;
            this.cbPerfStats.Text = "Include performance statistics in output";
            this.toolTip1.SetToolTip(this.cbPerfStats, "Adds some performance statistics to the end of the output");
            this.cbPerfStats.UseVisualStyleBackColor = true;
            // 
            // cbCaseSensitive
            // 
            this.cbCaseSensitive.AutoSize = true;
            this.cbCaseSensitive.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCaseSensitive.Location = new System.Drawing.Point(583, 142);
            this.cbCaseSensitive.Margin = new System.Windows.Forms.Padding(4);
            this.cbCaseSensitive.Name = "cbCaseSensitive";
            this.cbCaseSensitive.Size = new System.Drawing.Size(114, 22);
            this.cbCaseSensitive.TabIndex = 9;
            this.cbCaseSensitive.Text = "Case sensitive";
            this.toolTip1.SetToolTip(this.cbCaseSensitive, "Check to make the search sensitive to upper and lowercase letters");
            this.cbCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cbLineNos
            // 
            this.cbLineNos.AutoSize = true;
            this.cbLineNos.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLineNos.Location = new System.Drawing.Point(583, 112);
            this.cbLineNos.Margin = new System.Windows.Forms.Padding(4);
            this.cbLineNos.Name = "cbLineNos";
            this.cbLineNos.Size = new System.Drawing.Size(218, 22);
            this.cbLineNos.TabIndex = 8;
            this.cbLineNos.Text = "Include line numbers in output";
            this.toolTip1.SetToolTip(this.cbLineNos, "Adds \"@ ##\" to the end of each output line, indicating the line # within the file" +
                    " that your match was found");
            this.cbLineNos.UseVisualStyleBackColor = true;
            // 
            // rtbExcludes
            // 
            this.rtbExcludes.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExcludes.Location = new System.Drawing.Point(282, 113);
            this.rtbExcludes.Margin = new System.Windows.Forms.Padding(4);
            this.rtbExcludes.Name = "rtbExcludes";
            this.rtbExcludes.Size = new System.Drawing.Size(293, 110);
            this.rtbExcludes.TabIndex = 6;
            this.rtbExcludes.Text = "";
            this.toolTip1.SetToolTip(this.rtbExcludes, "Separate multiple terms with the ENTER key");
            // 
            // txbFileType
            // 
            this.txbFileType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbFileType.Location = new System.Drawing.Point(583, 48);
            this.txbFileType.Margin = new System.Windows.Forms.Padding(4);
            this.txbFileType.Name = "txbFileType";
            this.txbFileType.Size = new System.Drawing.Size(69, 26);
            this.txbFileType.TabIndex = 2;
            this.txbFileType.Text = "*.*";
            this.toolTip1.SetToolTip(this.txbFileType, "Only search within files that match this file type (ex: *.txt)");
            // 
            // rtbSearchTerms
            // 
            this.rtbSearchTerms.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSearchTerms.Location = new System.Drawing.Point(16, 113);
            this.rtbSearchTerms.Margin = new System.Windows.Forms.Padding(4);
            this.rtbSearchTerms.Name = "rtbSearchTerms";
            this.rtbSearchTerms.Size = new System.Drawing.Size(258, 110);
            this.rtbSearchTerms.TabIndex = 3;
            this.rtbSearchTerms.Text = "";
            this.toolTip1.SetToolTip(this.rtbSearchTerms, "Separate multiple terms with the ENTER key");
            this.rtbSearchTerms.WordWrap = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(535, 48);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(37, 26);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "...";
            this.toolTip1.SetToolTip(this.btnBrowse, "Browse for a folder in a separate window");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
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
            this.btnSearch.Location = new System.Drawing.Point(835, 0);
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
            this.btnTogglePreview.Location = new System.Drawing.Point(955, 0);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(53, 39);
            this.btnTogglePreview.TabIndex = 1;
            this.btnTogglePreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnTogglePreview, "Toggle preview");
            this.btnTogglePreview.UseVisualStyleBackColor = true;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(920, 21);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(81, 30);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnClear, "Clears the options, resetting them to the default values");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbFileExclude
            // 
            this.txbFileExclude.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbFileExclude.Location = new System.Drawing.Point(707, 49);
            this.txbFileExclude.Margin = new System.Windows.Forms.Padding(4);
            this.txbFileExclude.Name = "txbFileExclude";
            this.txbFileExclude.Size = new System.Drawing.Size(113, 26);
            this.txbFileExclude.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txbFileExclude, "Exclude files that match this filter (default = blank)");
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.grbBasicOptions);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 24);
            this.panTop.Margin = new System.Windows.Forms.Padding(4);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(1008, 239);
            this.panTop.TabIndex = 0;
            // 
            // grbBasicOptions
            // 
            this.grbBasicOptions.Controls.Add(this.txbFileExclude);
            this.grbBasicOptions.Controls.Add(this.lblFileExclude);
            this.grbBasicOptions.Controls.Add(this.cboSearchFolders);
            this.grbBasicOptions.Controls.Add(this.btnClear);
            this.grbBasicOptions.Controls.Add(this.cbIncludeSubfolders);
            this.grbBasicOptions.Controls.Add(this.lblInvalidNotice);
            this.grbBasicOptions.Controls.Add(this.cbPerfStats);
            this.grbBasicOptions.Controls.Add(this.cbCaseSensitive);
            this.grbBasicOptions.Controls.Add(this.cbLineNos);
            this.grbBasicOptions.Controls.Add(this.rtbExcludes);
            this.grbBasicOptions.Controls.Add(this.lblFileType);
            this.grbBasicOptions.Controls.Add(this.lblExcludes);
            this.grbBasicOptions.Controls.Add(this.txbFileType);
            this.grbBasicOptions.Controls.Add(this.lblSearchTerms);
            this.grbBasicOptions.Controls.Add(this.lblFolder);
            this.grbBasicOptions.Controls.Add(this.rtbSearchTerms);
            this.grbBasicOptions.Controls.Add(this.btnBrowse);
            this.grbBasicOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbBasicOptions.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbBasicOptions.Location = new System.Drawing.Point(0, 0);
            this.grbBasicOptions.Margin = new System.Windows.Forms.Padding(4);
            this.grbBasicOptions.Name = "grbBasicOptions";
            this.grbBasicOptions.Padding = new System.Windows.Forms.Padding(4);
            this.grbBasicOptions.Size = new System.Drawing.Size(1008, 239);
            this.grbBasicOptions.TabIndex = 0;
            this.grbBasicOptions.TabStop = false;
            this.grbBasicOptions.Text = "Search options";
            // 
            // lblFileExclude
            // 
            this.lblFileExclude.AutoSize = true;
            this.lblFileExclude.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileExclude.Location = new System.Drawing.Point(704, 26);
            this.lblFileExclude.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileExclude.Name = "lblFileExclude";
            this.lblFileExclude.Size = new System.Drawing.Size(116, 18);
            this.lblFileExclude.TabIndex = 14;
            this.lblFileExclude.Text = "File exclude filter";
            // 
            // cboSearchFolders
            // 
            this.cboSearchFolders.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSearchFolders.FormattingEnabled = true;
            this.cboSearchFolders.Location = new System.Drawing.Point(16, 48);
            this.cboSearchFolders.MaxDropDownItems = 20;
            this.cboSearchFolders.Name = "cboSearchFolders";
            this.cboSearchFolders.Size = new System.Drawing.Size(512, 26);
            this.cboSearchFolders.TabIndex = 0;
            this.cboSearchFolders.TextChanged += new System.EventHandler(this.txbDirectory_TextChanged);
            // 
            // lblInvalidNotice
            // 
            this.lblInvalidNotice.AutoSize = true;
            this.lblInvalidNotice.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvalidNotice.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidNotice.Location = new System.Drawing.Point(12, 78);
            this.lblInvalidNotice.Name = "lblInvalidNotice";
            this.lblInvalidNotice.Size = new System.Drawing.Size(132, 18);
            this.lblInvalidNotice.TabIndex = 12;
            this.lblInvalidNotice.Text = "This folder is invalid";
            this.lblInvalidNotice.Visible = false;
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileType.Location = new System.Drawing.Point(580, 26);
            this.lblFileType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(95, 18);
            this.lblFileType.TabIndex = 7;
            this.lblFileType.Text = "File type filter";
            // 
            // lblExcludes
            // 
            this.lblExcludes.AutoSize = true;
            this.lblExcludes.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcludes.Location = new System.Drawing.Point(282, 91);
            this.lblExcludes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExcludes.Name = "lblExcludes";
            this.lblExcludes.Size = new System.Drawing.Size(296, 18);
            this.lblExcludes.TabIndex = 5;
            this.lblExcludes.Text = "*Exclude* files that contain any of these words";
            // 
            // lblSearchTerms
            // 
            this.lblSearchTerms.AutoSize = true;
            this.lblSearchTerms.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchTerms.Location = new System.Drawing.Point(13, 91);
            this.lblSearchTerms.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchTerms.Name = "lblSearchTerms";
            this.lblSearchTerms.Size = new System.Drawing.Size(261, 18);
            this.lblSearchTerms.TabIndex = 5;
            this.lblSearchTerms.Text = "For files that contain *all* of these words";
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolder.Location = new System.Drawing.Point(13, 26);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(120, 18);
            this.lblFolder.TabIndex = 4;
            this.lblFolder.Text = "Search this folder:";
            // 
            // panMid
            // 
            this.panMid.Controls.Add(this.splitCont);
            this.panMid.Controls.Add(this.panLabel);
            this.panMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMid.Location = new System.Drawing.Point(0, 263);
            this.panMid.Margin = new System.Windows.Forms.Padding(4);
            this.panMid.Name = "panMid";
            this.panMid.Size = new System.Drawing.Size(1008, 315);
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
            // 
            // lbResults
            // 
            this.lbResults.ContextMenuStrip = this.contextMenuStrip1;
            this.lbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbResults.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResults.FormattingEnabled = true;
            this.lbResults.HorizontalScrollbar = true;
            this.lbResults.ItemHeight = 15;
            this.lbResults.Location = new System.Drawing.Point(0, 0);
            this.lbResults.Name = "lbResults";
            this.lbResults.Size = new System.Drawing.Size(273, 94);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(239, 114);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToClipboardToolStripMenuItem.Image")));
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy ALL rows to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // copySelectedRowToClipboardToolStripMenuItem
            // 
            this.copySelectedRowToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copySelectedRowToClipboardToolStripMenuItem.Image")));
            this.copySelectedRowToClipboardToolStripMenuItem.Name = "copySelectedRowToClipboardToolStripMenuItem";
            this.copySelectedRowToClipboardToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.copySelectedRowToClipboardToolStripMenuItem.Text = "Copy selected row to clipboard";
            this.copySelectedRowToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySelectedRowToClipboardToolStripMenuItem_Click);
            // 
            // openFileInDefaultEditorToolStripMenuItem
            // 
            this.openFileInDefaultEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFileInDefaultEditorToolStripMenuItem.Image")));
            this.openFileInDefaultEditorToolStripMenuItem.Name = "openFileInDefaultEditorToolStripMenuItem";
            this.openFileInDefaultEditorToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.openFileInDefaultEditorToolStripMenuItem.Text = "Open file in default editor";
            this.openFileInDefaultEditorToolStripMenuItem.Click += new System.EventHandler(this.openFileInDefaultEditorToolStripMenuItem_Click);
            // 
            // openEnclosingFolderToolStripMenuItem
            // 
            this.openEnclosingFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openEnclosingFolderToolStripMenuItem.Image")));
            this.openEnclosingFolderToolStripMenuItem.Name = "openEnclosingFolderToolStripMenuItem";
            this.openEnclosingFolderToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.openEnclosingFolderToolStripMenuItem.Text = "Open enclosing folder";
            this.openEnclosingFolderToolStripMenuItem.Click += new System.EventHandler(this.openEnclosingFolderToolStripMenuItem_Click);
            // 
            // openFileInCustomEditorToolStripMenuItem
            // 
            this.openFileInCustomEditorToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFileInCustomEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFileInCustomEditorToolStripMenuItem.Image")));
            this.openFileInCustomEditorToolStripMenuItem.Name = "openFileInCustomEditorToolStripMenuItem";
            this.openFileInCustomEditorToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
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
            this.panLabel.Controls.Add(this.lblProgress);
            this.panLabel.Controls.Add(this.btnTogglePreview);
            this.panLabel.Controls.Add(this.lblSrchRes);
            this.panLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panLabel.Location = new System.Drawing.Point(0, 0);
            this.panLabel.Margin = new System.Windows.Forms.Padding(4);
            this.panLabel.Name = "panLabel";
            this.panLabel.Size = new System.Drawing.Size(1008, 39);
            this.panLabel.TabIndex = 1;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.panBottom.Size = new System.Drawing.Size(1008, 81);
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
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSearchToolStripMenuItem,
            this.cancelSearchToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // runSearchToolStripMenuItem
            // 
            this.runSearchToolStripMenuItem.Name = "runSearchToolStripMenuItem";
            this.runSearchToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runSearchToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.runSearchToolStripMenuItem.Text = "Search";
            this.runSearchToolStripMenuItem.Click += new System.EventHandler(this.runSearchToolStripMenuItem_Click);
            // 
            // cancelSearchToolStripMenuItem
            // 
            this.cancelSearchToolStripMenuItem.Name = "cancelSearchToolStripMenuItem";
            this.cancelSearchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)));
            this.cancelSearchToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.cancelSearchToolStripMenuItem.Text = "Cancel Search";
            this.cancelSearchToolStripMenuItem.Visible = false;
            this.cancelSearchToolStripMenuItem.Click += new System.EventHandler(this.cancelSearchToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // oToolStripMenuItem
            // 
            this.oToolStripMenuItem.Name = "oToolStripMenuItem";
            this.oToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.oToolStripMenuItem.Text = "Options...";
            this.oToolStripMenuItem.Click += new System.EventHandler(this.oToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 578);
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
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ClientSizeChanged += new System.EventHandler(this.frmMain_ClientSizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.panTop.ResumeLayout(false);
            this.grbBasicOptions.ResumeLayout(false);
            this.grbBasicOptions.PerformLayout();
            this.panMid.ResumeLayout(false);
            this.splitCont.Panel1.ResumeLayout(false);
            this.splitCont.Panel2.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Timer timerRefreshGUI;
        private System.Windows.Forms.RichTextBox rtbSearchTerms;
        private System.Windows.Forms.Label lblSearchTerms;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.RichTextBox rtbExcludes;
        private System.Windows.Forms.Label lblExcludes;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.TextBox txbFileType;
        private System.Windows.Forms.CheckBox cbPerfStats;
        private System.Windows.Forms.CheckBox cbCaseSensitive;
        private System.Windows.Forms.CheckBox cbLineNos;
        private System.Windows.Forms.CheckBox cbIncludeSubfolders;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblInvalidNotice;
        private System.Windows.Forms.SplitContainer splitCont;
        private System.Windows.Forms.ListBox lbResults;
        private System.Windows.Forms.RichTextBox rtbExcerpt;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolStripMenuItem runSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelSearchToolStripMenuItem;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedRowToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileInDefaultEditorToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboSearchFolders;
        private System.Windows.Forms.ToolStripMenuItem openEnclosingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileInCustomEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oToolStripMenuItem;
        private System.Windows.Forms.TextBox txbFileExclude;
        private System.Windows.Forms.Label lblFileExclude;

    }
}

