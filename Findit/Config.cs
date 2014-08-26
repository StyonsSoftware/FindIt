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

        private void button3_Click(object sender, EventArgs e)
        {
            SaveConfigOptions();
            Close();
        }

        private void SaveConfigOptions()
        {
            gp.CustomEditorExe = txbCustomEditorExe.Text;
            gp.SaveToRegistry();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            LoadCurrentPreferences();
        }

        private void LoadCurrentPreferences()
        {
            txbCustomEditorExe.Text = gp.CustomEditorExe;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderClick(ref txbCustomEditorExe);
        }

        private void FolderClick(ref TextBox DisplayBox)
        {
            //show a folder browser dialog & tie it to whatever textbox they gave us
            var dlg = new OpenFileDialog();
            dlg.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            if (System.IO.File.Exists(DisplayBox.Text))
            {
                dlg.FileName = DisplayBox.Text;
            }
            else
            {
                dlg.FileName = "c:\\";
            }

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
    }
}
