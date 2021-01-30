namespace UareU.Kiosk
{
    partial class uc_timelog_mode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_timelog_mode));
            this.pbIn = new System.Windows.Forms.PictureBox();
            this.pbOut = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOut)).BeginInit();
            this.SuspendLayout();
            // 
            // pbIn
            // 
            this.pbIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbIn.Location = new System.Drawing.Point(8, 7);
            this.pbIn.Name = "pbIn";
            this.pbIn.Size = new System.Drawing.Size(200, 200);
            this.pbIn.TabIndex = 0;
            this.pbIn.TabStop = false;
            this.pbIn.Click += new System.EventHandler(this.pbIn_Click);
            // 
            // pbOut
            // 
            this.pbOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbOut.Location = new System.Drawing.Point(8, 239);
            this.pbOut.Name = "pbOut";
            this.pbOut.Size = new System.Drawing.Size(200, 200);
            this.pbOut.TabIndex = 1;
            this.pbOut.TabStop = false;
            this.pbOut.Click += new System.EventHandler(this.pbOut_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "button-login.png");
            this.imageList1.Images.SetKeyName(1, "button-login-active.png");
            this.imageList1.Images.SetKeyName(2, "button-logout.png");
            this.imageList1.Images.SetKeyName(3, "button-logout-active.png");
            // 
            // uc_timelog_mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pbOut);
            this.Controls.Add(this.pbIn);
            this.Name = "uc_timelog_mode";
            this.Size = new System.Drawing.Size(217, 447);
            this.Load += new System.EventHandler(this.uc_timelog_mode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbIn;
        private System.Windows.Forms.PictureBox pbOut;
        private System.Windows.Forms.ImageList imageList1;
    }
}
