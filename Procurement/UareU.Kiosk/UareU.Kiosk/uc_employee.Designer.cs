namespace UareU.Kiosk
{
    partial class uc_employee
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
            this.components = new System.ComponentModel.Container();
            this.pbEmployeePhoto = new System.Windows.Forms.PictureBox();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblOffice = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.tmrDisplay = new System.Windows.Forms.Timer(this.components);
            this.lblTimeLog = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uc_schedules1 = new UareU.Kiosk.uc_schedules();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbEmployeePhoto
            // 
            this.pbEmployeePhoto.BackColor = System.Drawing.Color.Transparent;
            this.pbEmployeePhoto.Image = global::UareU.Kiosk.Properties.Resources.user;
            this.pbEmployeePhoto.Location = new System.Drawing.Point(3, 3);
            this.pbEmployeePhoto.MaximumSize = new System.Drawing.Size(200, 200);
            this.pbEmployeePhoto.MinimumSize = new System.Drawing.Size(100, 100);
            this.pbEmployeePhoto.Name = "pbEmployeePhoto";
            this.pbEmployeePhoto.Size = new System.Drawing.Size(100, 100);
            this.pbEmployeePhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEmployeePhoto.TabIndex = 19;
            this.pbEmployeePhoto.TabStop = false;
            // 
            // lblPosition
            // 
            this.lblPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosition.ForeColor = System.Drawing.Color.White;
            this.lblPosition.Location = new System.Drawing.Point(121, 62);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(662, 27);
            this.lblPosition.TabIndex = 18;
            this.lblPosition.Text = "[position]";
            // 
            // lblOffice
            // 
            this.lblOffice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOffice.BackColor = System.Drawing.Color.Transparent;
            this.lblOffice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffice.ForeColor = System.Drawing.Color.White;
            this.lblOffice.Location = new System.Drawing.Point(121, 35);
            this.lblOffice.Name = "lblOffice";
            this.lblOffice.Size = new System.Drawing.Size(662, 27);
            this.lblOffice.TabIndex = 17;
            this.lblOffice.Text = "[office]";
            // 
            // lblEmployee
            // 
            this.lblEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmployee.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee.ForeColor = System.Drawing.Color.White;
            this.lblEmployee.Location = new System.Drawing.Point(120, 1);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(663, 34);
            this.lblEmployee.TabIndex = 16;
            this.lblEmployee.Text = "[employee]";
            // 
            // tmrDisplay
            // 
            this.tmrDisplay.Interval = 1000;
            this.tmrDisplay.Tick += new System.EventHandler(this.tmrDisplay_Tick);
            // 
            // lblTimeLog
            // 
            this.lblTimeLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeLog.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeLog.ForeColor = System.Drawing.SystemColors.Info;
            this.lblTimeLog.Location = new System.Drawing.Point(3, 0);
            this.lblTimeLog.Name = "lblTimeLog";
            this.lblTimeLog.Size = new System.Drawing.Size(404, 43);
            this.lblTimeLog.TabIndex = 20;
            this.lblTimeLog.Text = "[timelog]";
            this.lblTimeLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMode
            // 
            this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMode.BackColor = System.Drawing.Color.Transparent;
            this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.ForeColor = System.Drawing.Color.White;
            this.lblMode.Location = new System.Drawing.Point(3, 43);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(404, 43);
            this.lblMode.TabIndex = 21;
            this.lblMode.Text = "[mode]";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTimeLog);
            this.panel1.Controls.Add(this.lblMode);
            this.panel1.Location = new System.Drawing.Point(386, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 100);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbEmployeePhoto);
            this.panel2.Controls.Add(this.lblEmployee);
            this.panel2.Controls.Add(this.lblOffice);
            this.panel2.Controls.Add(this.lblPosition);
            this.panel2.Location = new System.Drawing.Point(10, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(786, 108);
            this.panel2.TabIndex = 24;
            // 
            // uc_schedules1
            // 
            this.uc_schedules1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uc_schedules1.BackColor = System.Drawing.Color.Transparent;
            this.uc_schedules1.Location = new System.Drawing.Point(10, 117);
            this.uc_schedules1.Name = "uc_schedules1";
            this.uc_schedules1.Size = new System.Drawing.Size(370, 173);
            this.uc_schedules1.TabIndex = 22;
            // 
            // uc_employee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.uc_schedules1);
            this.Name = "uc_employee";
            this.Size = new System.Drawing.Size(796, 293);
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployeePhoto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbEmployeePhoto;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblOffice;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Timer tmrDisplay;
        private System.Windows.Forms.Label lblTimeLog;
        private System.Windows.Forms.Label lblMode;
        private uc_schedules uc_schedules1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
