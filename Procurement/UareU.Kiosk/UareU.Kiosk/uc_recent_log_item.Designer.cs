namespace UareU.Kiosk
{
    partial class uc_recent_log_item
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTimeLog = new System.Windows.Forms.Label();
            this.pbEmployeePhoto = new System.Windows.Forms.PictureBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTimeLog
            // 
            this.lblTimeLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeLog.AutoSize = true;
            this.lblTimeLog.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeLog.ForeColor = System.Drawing.SystemColors.Info;
            this.lblTimeLog.Location = new System.Drawing.Point(80, 27);
            this.lblTimeLog.Name = "lblTimeLog";
            this.lblTimeLog.Size = new System.Drawing.Size(77, 20);
            this.lblTimeLog.TabIndex = 23;
            this.lblTimeLog.Text = "[timelog]";
            this.lblTimeLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbEmployeePhoto
            // 
            this.pbEmployeePhoto.BackColor = System.Drawing.Color.Transparent;
            this.pbEmployeePhoto.Location = new System.Drawing.Point(4, 3);
            this.pbEmployeePhoto.MaximumSize = new System.Drawing.Size(70, 70);
            this.pbEmployeePhoto.MinimumSize = new System.Drawing.Size(70, 70);
            this.pbEmployeePhoto.Name = "pbEmployeePhoto";
            this.pbEmployeePhoto.Size = new System.Drawing.Size(70, 70);
            this.pbEmployeePhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEmployeePhoto.TabIndex = 22;
            this.pbEmployeePhoto.TabStop = false;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee.ForeColor = System.Drawing.Color.White;
            this.lblEmployee.Location = new System.Drawing.Point(80, 3);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(85, 20);
            this.lblEmployee.TabIndex = 21;
            this.lblEmployee.Text = "[employee]";
            // 
            // lblMode
            // 
            this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.Transparent;
            this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.ForeColor = System.Drawing.Color.White;
            this.lblMode.Location = new System.Drawing.Point(80, 51);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(57, 20);
            this.lblMode.TabIndex = 24;
            this.lblMode.Text = "[mode]";
            // 
            // uc_recent_log_item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.lblTimeLog);
            this.Controls.Add(this.pbEmployeePhoto);
            this.Controls.Add(this.lblEmployee);
            this.Name = "uc_recent_log_item";
            this.Size = new System.Drawing.Size(313, 77);
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTimeLog;
        private System.Windows.Forms.PictureBox pbEmployeePhoto;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblMode;
    }
}
