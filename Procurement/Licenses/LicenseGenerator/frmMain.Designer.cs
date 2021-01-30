namespace LicenseGenerator
{
    partial class frmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbMotherboardSerialNo = new System.Windows.Forms.TextBox();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tbClientName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dtpExpiry = new System.Windows.Forms.DateTimePicker();
            this.cbShowExpiry = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbUnlimited = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbAsposePdf = new System.Windows.Forms.CheckBox();
            this.cbAsposeBarcode = new System.Windows.Forms.CheckBox();
            this.cbAsposeCells = new System.Windows.Forms.CheckBox();
            this.cbAsposeWords = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbSAM = new System.Windows.Forms.CheckBox();
            this.cbProcurement = new System.Windows.Forms.CheckBox();
            this.tbApplicationName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPLDTMP = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Motherboard Serial No.";
            // 
            // tbMotherboardSerialNo
            // 
            this.tbMotherboardSerialNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMotherboardSerialNo.Location = new System.Drawing.Point(12, 87);
            this.tbMotherboardSerialNo.Name = "tbMotherboardSerialNo";
            this.tbMotherboardSerialNo.Size = new System.Drawing.Size(434, 20);
            this.tbMotherboardSerialNo.TabIndex = 1;
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputPath.Location = new System.Drawing.Point(6, 19);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(341, 20);
            this.tbOutputPath.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(353, 16);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(365, 489);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tbClientName
            // 
            this.tbClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClientName.Location = new System.Drawing.Point(12, 126);
            this.tbClientName.Name = "tbClientName";
            this.tbClientName.Size = new System.Drawing.Size(434, 20);
            this.tbClientName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Client Name";
            // 
            // dtpExpiry
            // 
            this.dtpExpiry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpExpiry.Location = new System.Drawing.Point(125, 17);
            this.dtpExpiry.Name = "dtpExpiry";
            this.dtpExpiry.Size = new System.Drawing.Size(303, 20);
            this.dtpExpiry.TabIndex = 9;
            // 
            // cbShowExpiry
            // 
            this.cbShowExpiry.AutoSize = true;
            this.cbShowExpiry.Location = new System.Drawing.Point(6, 45);
            this.cbShowExpiry.Name = "cbShowExpiry";
            this.cbShowExpiry.Size = new System.Drawing.Size(116, 17);
            this.cbShowExpiry.TabIndex = 10;
            this.cbShowExpiry.Text = "Show on Info page";
            this.cbShowExpiry.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbOutputPath);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Location = new System.Drawing.Point(12, 411);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 54);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output Path";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbUnlimited);
            this.groupBox2.Controls.Add(this.cbShowExpiry);
            this.groupBox2.Controls.Add(this.dtpExpiry);
            this.groupBox2.Location = new System.Drawing.Point(15, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(434, 74);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Expiry Date";
            // 
            // cbUnlimited
            // 
            this.cbUnlimited.AutoSize = true;
            this.cbUnlimited.Location = new System.Drawing.Point(6, 22);
            this.cbUnlimited.Name = "cbUnlimited";
            this.cbUnlimited.Size = new System.Drawing.Size(69, 17);
            this.cbUnlimited.TabIndex = 11;
            this.cbUnlimited.Text = "Unlimited";
            this.cbUnlimited.UseVisualStyleBackColor = true;
            this.cbUnlimited.CheckedChanged += new System.EventHandler(this.cbUnlimited_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(458, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.cbAsposePdf);
            this.groupBox3.Controls.Add(this.cbAsposeBarcode);
            this.groupBox3.Controls.Add(this.cbAsposeCells);
            this.groupBox3.Controls.Add(this.cbAsposeWords);
            this.groupBox3.Location = new System.Drawing.Point(15, 251);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(434, 69);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Aspose Licenses";
            // 
            // cbAsposePdf
            // 
            this.cbAsposePdf.AutoSize = true;
            this.cbAsposePdf.Location = new System.Drawing.Point(98, 42);
            this.cbAsposePdf.Name = "cbAsposePdf";
            this.cbAsposePdf.Size = new System.Drawing.Size(42, 17);
            this.cbAsposePdf.TabIndex = 15;
            this.cbAsposePdf.Text = "Pdf";
            this.cbAsposePdf.UseVisualStyleBackColor = true;
            // 
            // cbAsposeBarcode
            // 
            this.cbAsposeBarcode.AutoSize = true;
            this.cbAsposeBarcode.Location = new System.Drawing.Point(98, 19);
            this.cbAsposeBarcode.Name = "cbAsposeBarcode";
            this.cbAsposeBarcode.Size = new System.Drawing.Size(66, 17);
            this.cbAsposeBarcode.TabIndex = 14;
            this.cbAsposeBarcode.Text = "Barcode";
            this.cbAsposeBarcode.UseVisualStyleBackColor = true;
            // 
            // cbAsposeCells
            // 
            this.cbAsposeCells.AutoSize = true;
            this.cbAsposeCells.Location = new System.Drawing.Point(6, 42);
            this.cbAsposeCells.Name = "cbAsposeCells";
            this.cbAsposeCells.Size = new System.Drawing.Size(48, 17);
            this.cbAsposeCells.TabIndex = 12;
            this.cbAsposeCells.Text = "Cells";
            this.cbAsposeCells.UseVisualStyleBackColor = true;
            // 
            // cbAsposeWords
            // 
            this.cbAsposeWords.AutoSize = true;
            this.cbAsposeWords.Location = new System.Drawing.Point(6, 19);
            this.cbAsposeWords.Name = "cbAsposeWords";
            this.cbAsposeWords.Size = new System.Drawing.Size(57, 17);
            this.cbAsposeWords.TabIndex = 11;
            this.cbAsposeWords.Text = "Words";
            this.cbAsposeWords.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.cbPLDTMP);
            this.groupBox4.Controls.Add(this.cbSAM);
            this.groupBox4.Controls.Add(this.cbProcurement);
            this.groupBox4.Location = new System.Drawing.Point(15, 326);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(434, 53);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Modules";
            // 
            // cbSAM
            // 
            this.cbSAM.AutoSize = true;
            this.cbSAM.Location = new System.Drawing.Point(98, 19);
            this.cbSAM.Name = "cbSAM";
            this.cbSAM.Size = new System.Drawing.Size(159, 17);
            this.cbSAM.TabIndex = 13;
            this.cbSAM.Text = "Supply/Assets Management";
            this.cbSAM.UseVisualStyleBackColor = true;
            // 
            // cbProcurement
            // 
            this.cbProcurement.AutoSize = true;
            this.cbProcurement.Location = new System.Drawing.Point(6, 19);
            this.cbProcurement.Name = "cbProcurement";
            this.cbProcurement.Size = new System.Drawing.Size(86, 17);
            this.cbProcurement.TabIndex = 11;
            this.cbProcurement.Text = "Procurement";
            this.cbProcurement.UseVisualStyleBackColor = true;
            // 
            // tbApplicationName
            // 
            this.tbApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApplicationName.Location = new System.Drawing.Point(12, 48);
            this.tbApplicationName.Name = "tbApplicationName";
            this.tbApplicationName.Size = new System.Drawing.Size(434, 20);
            this.tbApplicationName.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Application Name";
            // 
            // cbPLDTMP
            // 
            this.cbPLDTMP.AutoSize = true;
            this.cbPLDTMP.Location = new System.Drawing.Point(263, 19);
            this.cbPLDTMP.Name = "cbPLDTMP";
            this.cbPLDTMP.Size = new System.Drawing.Size(73, 17);
            this.cbPLDTMP.TabIndex = 14;
            this.cbPLDTMP.Text = "PLDT-MP";
            this.cbPLDTMP.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 524);
            this.Controls.Add(this.tbApplicationName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbClientName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.tbMotherboardSerialNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "License Generator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMotherboardSerialNo;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbClientName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.CheckBox cbShowExpiry;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbUnlimited;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbAsposeCells;
        private System.Windows.Forms.CheckBox cbAsposeWords;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbProcurement;
        private System.Windows.Forms.TextBox tbApplicationName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbAsposeBarcode;
        private System.Windows.Forms.CheckBox cbSAM;
        private System.Windows.Forms.CheckBox cbAsposePdf;
        private System.Windows.Forms.CheckBox cbPLDTMP;
    }
}

