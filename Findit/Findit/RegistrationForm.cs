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
    public partial class RegistrationForm : Form
    {
        Registration regInfo;

        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            regInfo = new Registration();
            if (!regInfo.WithinTrialPeriod())
            {
                lblMsg.Text = "The 30 day trial period has expired." + Environment.NewLine;
            }
            lblMsg.Text = "Enter your registration code here:";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AttemptKeyAccept();
        }

        private void AttemptKeyAccept()
        {
            if (regInfo.ValidKey(txbKey.Text))
            {
                regInfo.StoreValidation(txbKey.Text);
                MessageBox.Show("Thank you for registering!");
                Close();
            }
            else
            {
                if (regInfo.PaidFor())
                {
                    MessageBox.Show("That key is not valid, but you already paid so it's OK.");
                }
                else
                {
                    MessageBox.Show("That key is not valid.");
                    regInfo.StoreValidation("not registered");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txbKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)
            {
                AttemptKeyAccept();
            }
        }
    }
}
