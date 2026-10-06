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
      NPC.Licensing.LicenseKeyHelper lkh = new NPC.Licensing.LicenseKeyHelper();
      if (lkh.ProductFromKey(txbKey.Text) == "FindIt")
      {
        RegistrationPreferences rp = new RegistrationPreferences();
        rp.RegistrationKey = txbKey.Text;
        rp.SaveToRegistry();
      }
      else
      {
        MessageBox.Show("Sorry, that key is not valid.  Please refer to the email you received when you purchased FindIt.");
        txbKey.Clear();
      }
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
