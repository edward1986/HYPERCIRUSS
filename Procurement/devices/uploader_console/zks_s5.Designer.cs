namespace zks_console
{
    partial class zks_s5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(zks_s5));
            this.axSB100BPC1 = new AxSB100BPCLib.AxSB100BPC();
            ((System.ComponentModel.ISupportInitialize)(this.axSB100BPC1)).BeginInit();
            this.SuspendLayout();
            // 
            // axSB100BPC1
            // 
            this.axSB100BPC1.Enabled = true;
            this.axSB100BPC1.Location = new System.Drawing.Point(0, 0);
            this.axSB100BPC1.Name = "axSB100BPC1";
            this.axSB100BPC1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSB100BPC1.OcxState")));
            this.axSB100BPC1.Size = new System.Drawing.Size(100, 50);
            this.axSB100BPC1.TabIndex = 0;
            // 
            // zks_s5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.axSB100BPC1);
            this.Name = "zks_s5";
            this.Text = "zks_s5";
            ((System.ComponentModel.ISupportInitialize)(this.axSB100BPC1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxSB100BPCLib.AxSB100BPC axSB100BPC1;
    }
}