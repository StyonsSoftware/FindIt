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
    public partial class PurchaseOptions : Form
    {
        public PurchaseOptions()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            SetRegistrationMessages();
        }

        private void SetRegistrationMessages()
        {
            Registration regInfo = new Registration();
            if (!regInfo.PaidFor())
            {
                lblRegistrationStatus.Text = "NOT REGISTERED";
                if (regInfo.WithinTrialPeriod())
                {
                    lblTrialPeriodInfo.Text = "You have " + regInfo.DaysLeftInTrialPeriod().ToString() + " days left in your trial period.";
                }
                else
                {
                    lblTrialPeriodInfo.Text = "The trial period has expired." + Environment.NewLine + 
                        "Findit's performance will be severely reduced"+ Environment.NewLine +
                        "until you enter a valid key.";
                }
            }
            else
            {
                lblRegistrationStatus.Text = "Thank you for registering!";
                lblTrialPeriodInfo.Text = "";
                btnCancel.Text = "Close";
            }

        }

        private void btnEnterKey_Click(object sender, EventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm();
            regForm.StartPosition = FormStartPosition.CenterParent;
            regForm.ShowDialog();
            SetRegistrationMessages();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
