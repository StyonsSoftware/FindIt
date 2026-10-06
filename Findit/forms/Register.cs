using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Findit.forms
{
  public partial class frmRegister : Form
  {
    public frmRegister()
    {
      InitializeComponent();
    }

    private void linkQuestions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ProcessStartInfo sInfo = new ProcessStartInfo("https://www.nonprofit-complete.com/docs/findit.html");
      Process.Start(sInfo);
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
      if (Licensing.IsValidKey(txbKey.Text))
      {
        RegistrationPreferences rp = new RegistrationPreferences();
        rp.RegistrationKey = txbKey.Text;
        rp.SaveToRegistry();
        rp.Dispose();
        MessageBox.Show(this,"Thank you for registering FindIt!");
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        MessageBox.Show(this,"Sorry, that key is not valid.  Please refer to the email you received when you purchased FindIt.","Register",
          MessageBoxButtons.OK,MessageBoxIcon.Warning);
        txbKey.Clear();
        txbKey.Focus();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
