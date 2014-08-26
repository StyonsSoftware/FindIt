namespace Findit
{
    partial class PurchaseOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOptions));
            this.lblRegistrationStatus = new System.Windows.Forms.Label();
            this.lblTrialPeriodInfo = new System.Windows.Forms.Label();
            this.btnBuy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEnterKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRegistrationStatus
            // 
            this.lblRegistrationStatus.AutoSize = true;
            this.lblRegistrationStatus.Location = new System.Drawing.Point(33, 9);
            this.lblRegistrationStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegistrationStatus.Name = "lblRegistrationStatus";
            this.lblRegistrationStatus.Size = new System.Drawing.Size(228, 18);
            this.lblRegistrationStatus.TabIndex = 0;
            this.lblRegistrationStatus.Text = "This copy of FindIt is not registered.";
            // 
            // lblTrialPeriodInfo
            // 
            this.lblTrialPeriodInfo.AutoSize = true;
            this.lblTrialPeriodInfo.Location = new System.Drawing.Point(33, 51);
            this.lblTrialPeriodInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrialPeriodInfo.Name = "lblTrialPeriodInfo";
            this.lblTrialPeriodInfo.Size = new System.Drawing.Size(257, 18);
            this.lblTrialPeriodInfo.TabIndex = 1;
            this.lblTrialPeriodInfo.Text = "You have 30 days left in your trial period.";
            // 
            // btnBuy
            // 
            this.btnBuy.Image = ((System.Drawing.Image)(resources.GetObject("btnBuy.Image")));
            this.btnBuy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuy.Location = new System.Drawing.Point(36, 216);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(254, 32);
            this.btnBuy.TabIndex = 4;
            this.btnBuy.Text = "Purchase a Key...";
            this.btnBuy.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(36, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(254, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Not now";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEnterKey
            // 
            this.btnEnterKey.Image = ((System.Drawing.Image)(resources.GetObject("btnEnterKey.Image")));
            this.btnEnterKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnterKey.Location = new System.Drawing.Point(36, 139);
            this.btnEnterKey.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnterKey.Name = "btnEnterKey";
            this.btnEnterKey.Size = new System.Drawing.Size(254, 32);
            this.btnEnterKey.TabIndex = 2;
            this.btnEnterKey.Text = "Enter a key...";
            this.btnEnterKey.UseVisualStyleBackColor = true;
            this.btnEnterKey.Click += new System.EventHandler(this.btnEnterKey_Click);
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 265);
            this.Controls.Add(this.btnBuy);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEnterKey);
            this.Controls.Add(this.lblTrialPeriodInfo);
            this.Controls.Add(this.lblRegistrationStatus);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Register";
            this.Text = "Register";
            this.Load += new System.EventHandler(this.Register_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRegistrationStatus;
        private System.Windows.Forms.Label lblTrialPeriodInfo;
        private System.Windows.Forms.Button btnEnterKey;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBuy;
    }
}