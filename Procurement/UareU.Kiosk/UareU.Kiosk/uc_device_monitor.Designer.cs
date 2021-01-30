namespace UareU.Kiosk
{
    partial class uc_device_monitor
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tmrReconnect = new System.Windows.Forms.Timer(this.components);
            this.lblReconnectStatus = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(3, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(55, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "[message]";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 36);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "[status]";
            // 
            // tmrReconnect
            // 
            this.tmrReconnect.Interval = 1000;
            this.tmrReconnect.Tick += new System.EventHandler(this.tmrReconnect_Tick);
            // 
            // lblReconnectStatus
            // 
            this.lblReconnectStatus.AutoSize = true;
            this.lblReconnectStatus.Location = new System.Drawing.Point(3, 49);
            this.lblReconnectStatus.Name = "lblReconnectStatus";
            this.lblReconnectStatus.Size = new System.Drawing.Size(92, 13);
            this.lblReconnectStatus.TabIndex = 2;
            this.lblReconnectStatus.Text = "[reconnect status]";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(3, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(33, 13);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "[type]";
            // 
            // uc_device_monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblReconnectStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblMessage);
            this.Name = "uc_device_monitor";
            this.Size = new System.Drawing.Size(214, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer tmrReconnect;
        private System.Windows.Forms.Label lblReconnectStatus;
        private System.Windows.Forms.Label lblType;
    }
}
