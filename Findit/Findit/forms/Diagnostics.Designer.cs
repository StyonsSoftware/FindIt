namespace Findit
{
    partial class Diagnostics
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
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbFileQueues = new System.Windows.Forms.RichTextBox();
            this.lblThreadStates = new System.Windows.Forms.Label();
            this.rtbThreadStates = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "File queues (to be searched):";
            // 
            // rtbFileQueues
            // 
            this.rtbFileQueues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbFileQueues.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbFileQueues.Location = new System.Drawing.Point(13, 88);
            this.rtbFileQueues.Name = "rtbFileQueues";
            this.rtbFileQueues.Size = new System.Drawing.Size(881, 194);
            this.rtbFileQueues.TabIndex = 2;
            this.rtbFileQueues.Text = "";
            this.rtbFileQueues.WordWrap = false;
            // 
            // lblThreadStates
            // 
            this.lblThreadStates.AutoSize = true;
            this.lblThreadStates.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThreadStates.Location = new System.Drawing.Point(12, 324);
            this.lblThreadStates.Name = "lblThreadStates";
            this.lblThreadStates.Size = new System.Drawing.Size(125, 20);
            this.lblThreadStates.TabIndex = 3;
            this.lblThreadStates.Text = "Thread states:";
            // 
            // rtbThreadStates
            // 
            this.rtbThreadStates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbThreadStates.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbThreadStates.Location = new System.Drawing.Point(16, 347);
            this.rtbThreadStates.Name = "rtbThreadStates";
            this.rtbThreadStates.Size = new System.Drawing.Size(881, 259);
            this.rtbThreadStates.TabIndex = 4;
            this.rtbThreadStates.Text = "";
            this.rtbThreadStates.WordWrap = false;
            // 
            // Diagnostics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 618);
            this.Controls.Add(this.rtbThreadStates);
            this.Controls.Add(this.lblThreadStates);
            this.Controls.Add(this.rtbFileQueues);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Name = "Diagnostics";
            this.Text = "Diagnostics";
            this.Load += new System.EventHandler(this.Diagnostics_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbFileQueues;
        private System.Windows.Forms.Label lblThreadStates;
        private System.Windows.Forms.RichTextBox rtbThreadStates;
    }
}