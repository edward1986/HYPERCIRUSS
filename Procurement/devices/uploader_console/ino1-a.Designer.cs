namespace zks_console
{
    partial class ino1_a
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ino1_a));
            this.axCZKEM1 = new Axzkemkeeper.AxCZKEM();
            ((System.ComponentModel.ISupportInitialize)(this.axCZKEM1)).BeginInit();
            this.SuspendLayout();
            // 
            // axCZKEM1
            // 
            this.axCZKEM1.Enabled = true;
            this.axCZKEM1.Location = new System.Drawing.Point(0, 0);
            this.axCZKEM1.Name = "axCZKEM1";
            this.axCZKEM1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCZKEM1.OcxState")));
            this.axCZKEM1.Size = new System.Drawing.Size(192, 192);
            this.axCZKEM1.TabIndex = 0;
            // 
            // ino1_a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.axCZKEM1);
            this.Name = "ino1_a";
            this.Text = "ino1_a";
            ((System.ComponentModel.ISupportInitialize)(this.axCZKEM1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Axzkemkeeper.AxCZKEM axCZKEM1;
    }
}