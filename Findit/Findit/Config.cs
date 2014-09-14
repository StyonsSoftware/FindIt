using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Findit
{
    public partial class Config : Form
    {
        private GUIPreferences gp = new GUIPreferences();

        public Config()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfigOptions();
            Close();
        }

        private void SaveConfigOptions()
        {
            gp.CustomEditorExe = txbCustomEditorExe.Text;
            gp.RunSearchesAfterLoad = cbRunSearchAfterLoad.Checked;
            gp.SaveToRegistry();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            LoadCurrentPreferences();
            lblRecommendThreadCount.Text = "Recommended: " + Globals.RecommendedSearchThreadCount;
        }

        private void LoadCurrentPreferences()
        {
            txbCustomEditorExe.Text = gp.CustomEditorExe;
            cbRunSearchAfterLoad.Checked = gp.RunSearchesAfterLoad;
            tbarThreadCount.Value = gp.SearchThreadCount;
            RefreshThreadCountLabel();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderClick(ref txbCustomEditorExe, "exe files (*.exe)|*.exe|All files (*.*)|*.*");
        }

        private void FolderClick(ref TextBox DisplayBox, string filter)
        {           
            //show a folder browser dialog & tie it to whatever textbox they gave us
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter;
            if (System.IO.File.Exists(DisplayBox.Text))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(DisplayBox.Text);
                dlg.FileName = DisplayBox.Text;
            }
            else
            {
                dlg.FileName = "c:\\";
            }
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DisplayBox.Text = dlg.FileName;
            }
        }

        private void txbCustomEditorExe_TextChanged(object sender, EventArgs e)
        {
            HighlightInvalidFile();
        }

        private void HighlightInvalidFile()
        {
            if (System.IO.File.Exists(txbCustomEditorExe.Text))
            {
                txbCustomEditorExe.BackColor = txbColor.BackColor;
                lblInvalidNotice.Visible = false;
            }
            else
            {
                txbCustomEditorExe.BackColor = Color.Yellow;
                lblInvalidNotice.Visible = true;
            }
        }

        private void RefreshThreadCountLabel()
        {
            lblSelectedThreadCount.Text = tbarThreadCount.Value.ToString();
            if (tbarThreadCount.Value > Globals.RecommendedSearchThreadCount)
            {
                lblSelectedThreadCount.ForeColor = Color.Red;
                lblRecommendThreadCount.ForeColor = Color.Red;
            }
            else
            {
                lblSelectedThreadCount.ForeColor = Color.Black;
                lblRecommendThreadCount.ForeColor = Color.Black;
            }
            gp.SearchThreadCount = tbarThreadCount.Value;
        }

        private void tbarThreadCount_ValueChanged(object sender, EventArgs e)
        {
            RefreshThreadCountLabel();
        }
    }
}
