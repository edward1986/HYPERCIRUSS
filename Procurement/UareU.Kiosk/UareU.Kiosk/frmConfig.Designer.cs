namespace UareU.Kiosk
{
    partial class frmConfig
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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLongTimeFormat = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnForceQueryTime = new System.Windows.Forms.Button();
            this.cbUseServerTime = new System.Windows.Forms.CheckBox();
            this.nudServerTimeQueryInterval = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDownloadLogo = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAppUrl = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbOfflineMsg = new System.Windows.Forms.TextBox();
            this.btnApplyAndClose = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudTerminalId = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblDSVersion = new System.Windows.Forms.Label();
            this.btnBrowseODL = new System.Windows.Forms.Button();
            this.tbODL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowseTLL = new System.Windows.Forms.Button();
            this.tbTLL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbDataDownloadEnabled = new System.Windows.Forms.CheckBox();
            this.btnForceDownload = new System.Windows.Forms.Button();
            this.nudDataDownloadInterval = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnForcePost = new System.Windows.Forms.Button();
            this.nudPostOfflineLogsInterval = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cbFullview = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.nudMaxLogDisplayTime = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.cbEnableLANDevice = new System.Windows.Forms.CheckBox();
            this.cbEnableUSBDevice = new System.Windows.Forms.CheckBox();
            this.nudDeviceReconnectInterval = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.btnDownloadPhotos = new System.Windows.Forms.Button();
            this.lblPhotosVersion = new System.Windows.Forms.Label();
            this.btnBrowsePL = new System.Windows.Forms.Button();
            this.tbPL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerTimeQueryInterval)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTerminalId)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataDownloadInterval)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPostOfflineLogsInterval)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLogDisplayTime)).BeginInit();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeviceReconnectInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(360, 658);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(279, 658);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Interval (in minutes)";
            // 
            // cbLongTimeFormat
            // 
            this.cbLongTimeFormat.AutoSize = true;
            this.cbLongTimeFormat.Location = new System.Drawing.Point(17, 64);
            this.cbLongTimeFormat.Name = "cbLongTimeFormat";
            this.cbLongTimeFormat.Size = new System.Drawing.Size(110, 17);
            this.cbLongTimeFormat.TabIndex = 6;
            this.cbLongTimeFormat.Text = "Long Time Formal";
            this.cbLongTimeFormat.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnForceQueryTime);
            this.groupBox1.Controls.Add(this.cbUseServerTime);
            this.groupBox1.Controls.Add(this.nudServerTimeQueryInterval);
            this.groupBox1.Controls.Add(this.cbLongTimeFormat);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 128);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Time Query";
            // 
            // btnForceQueryTime
            // 
            this.btnForceQueryTime.Location = new System.Drawing.Point(17, 99);
            this.btnForceQueryTime.Name = "btnForceQueryTime";
            this.btnForceQueryTime.Size = new System.Drawing.Size(110, 23);
            this.btnForceQueryTime.TabIndex = 9;
            this.btnForceQueryTime.Text = "Force query";
            this.btnForceQueryTime.UseVisualStyleBackColor = true;
            this.btnForceQueryTime.Click += new System.EventHandler(this.btnForceQueryTime_Click);
            // 
            // cbUseServerTime
            // 
            this.cbUseServerTime.AutoSize = true;
            this.cbUseServerTime.Location = new System.Drawing.Point(17, 81);
            this.cbUseServerTime.Name = "cbUseServerTime";
            this.cbUseServerTime.Size = new System.Drawing.Size(105, 17);
            this.cbUseServerTime.TabIndex = 8;
            this.cbUseServerTime.Text = "Use Server Time";
            this.cbUseServerTime.UseVisualStyleBackColor = true;
            // 
            // nudServerTimeQueryInterval
            // 
            this.nudServerTimeQueryInterval.Location = new System.Drawing.Point(17, 38);
            this.nudServerTimeQueryInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudServerTimeQueryInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudServerTimeQueryInterval.Name = "nudServerTimeQueryInterval";
            this.nudServerTimeQueryInterval.Size = new System.Drawing.Size(110, 20);
            this.nudServerTimeQueryInterval.TabIndex = 7;
            this.nudServerTimeQueryInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudServerTimeQueryInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDownloadLogo);
            this.groupBox2.Controls.Add(this.pbLogo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbAppUrl);
            this.groupBox2.Location = new System.Drawing.Point(170, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 172);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SkyHR";
            // 
            // btnDownloadLogo
            // 
            this.btnDownloadLogo.Location = new System.Drawing.Point(102, 134);
            this.btnDownloadLogo.Name = "btnDownloadLogo";
            this.btnDownloadLogo.Size = new System.Drawing.Size(104, 23);
            this.btnDownloadLogo.TabIndex = 8;
            this.btnDownloadLogo.Text = "Download Logo";
            this.btnDownloadLogo.UseVisualStyleBackColor = true;
            this.btnDownloadLogo.Click += new System.EventHandler(this.btnDownloadLogo_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Location = new System.Drawing.Point(19, 80);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(77, 77);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 7;
            this.pbLogo.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Logo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Url";
            // 
            // tbAppUrl
            // 
            this.tbAppUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAppUrl.Location = new System.Drawing.Point(19, 41);
            this.tbAppUrl.Name = "tbAppUrl";
            this.tbAppUrl.Size = new System.Drawing.Size(334, 20);
            this.tbAppUrl.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbOfflineMsg);
            this.groupBox3.Location = new System.Drawing.Point(170, 423);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(370, 106);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Offline Message";
            // 
            // tbOfflineMsg
            // 
            this.tbOfflineMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOfflineMsg.Location = new System.Drawing.Point(17, 25);
            this.tbOfflineMsg.Multiline = true;
            this.tbOfflineMsg.Name = "tbOfflineMsg";
            this.tbOfflineMsg.Size = new System.Drawing.Size(334, 62);
            this.tbOfflineMsg.TabIndex = 4;
            // 
            // btnApplyAndClose
            // 
            this.btnApplyAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyAndClose.Location = new System.Drawing.Point(441, 658);
            this.btnApplyAndClose.Name = "btnApplyAndClose";
            this.btnApplyAndClose.Size = new System.Drawing.Size(99, 23);
            this.btnApplyAndClose.TabIndex = 10;
            this.btnApplyAndClose.Text = "Apply and Close";
            this.btnApplyAndClose.UseVisualStyleBackColor = true;
            this.btnApplyAndClose.Click += new System.EventHandler(this.btnApplyAndClose_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudTerminalId);
            this.groupBox4.Location = new System.Drawing.Point(15, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(140, 52);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Terminal Id";
            // 
            // nudTerminalId
            // 
            this.nudTerminalId.Location = new System.Drawing.Point(17, 19);
            this.nudTerminalId.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTerminalId.Name = "nudTerminalId";
            this.nudTerminalId.Size = new System.Drawing.Size(110, 20);
            this.nudTerminalId.TabIndex = 8;
            this.nudTerminalId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudTerminalId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblPhotosVersion);
            this.groupBox5.Controls.Add(this.btnBrowsePL);
            this.groupBox5.Controls.Add(this.btnDownloadPhotos);
            this.groupBox5.Controls.Add(this.tbPL);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.lblDSVersion);
            this.groupBox5.Controls.Add(this.btnBrowseODL);
            this.groupBox5.Controls.Add(this.tbODL);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.btnBrowseTLL);
            this.groupBox5.Controls.Add(this.tbTLL);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(170, 190);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(370, 218);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Folder Locations";
            // 
            // lblDSVersion
            // 
            this.lblDSVersion.AutoSize = true;
            this.lblDSVersion.Location = new System.Drawing.Point(16, 116);
            this.lblDSVersion.Name = "lblDSVersion";
            this.lblDSVersion.Size = new System.Drawing.Size(41, 13);
            this.lblDSVersion.TabIndex = 11;
            this.lblDSVersion.Text = "[empty]";
            // 
            // btnBrowseODL
            // 
            this.btnBrowseODL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseODL.Location = new System.Drawing.Point(276, 88);
            this.btnBrowseODL.Name = "btnBrowseODL";
            this.btnBrowseODL.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseODL.TabIndex = 9;
            this.btnBrowseODL.Text = "Browse...";
            this.btnBrowseODL.UseVisualStyleBackColor = true;
            this.btnBrowseODL.Click += new System.EventHandler(this.btnBrowseODL_Click);
            // 
            // tbODL
            // 
            this.tbODL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbODL.Location = new System.Drawing.Point(17, 91);
            this.tbODL.Name = "tbODL";
            this.tbODL.Size = new System.Drawing.Size(253, 20);
            this.tbODL.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Offline Data Source Location";
            // 
            // btnBrowseTLL
            // 
            this.btnBrowseTLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTLL.Location = new System.Drawing.Point(276, 34);
            this.btnBrowseTLL.Name = "btnBrowseTLL";
            this.btnBrowseTLL.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTLL.TabIndex = 6;
            this.btnBrowseTLL.Text = "Browse...";
            this.btnBrowseTLL.UseVisualStyleBackColor = true;
            this.btnBrowseTLL.Click += new System.EventHandler(this.btnBrowseTLL_Click);
            // 
            // tbTLL
            // 
            this.tbTLL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTLL.Location = new System.Drawing.Point(17, 37);
            this.tbTLL.Name = "tbTLL";
            this.tbTLL.Size = new System.Drawing.Size(253, 20);
            this.tbTLL.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Time Logs Location";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbDataDownloadEnabled);
            this.groupBox6.Controls.Add(this.btnForceDownload);
            this.groupBox6.Controls.Add(this.nudDataDownloadInterval);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Location = new System.Drawing.Point(15, 204);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(140, 128);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Data Download";
            // 
            // cbDataDownloadEnabled
            // 
            this.cbDataDownloadEnabled.AutoSize = true;
            this.cbDataDownloadEnabled.Location = new System.Drawing.Point(17, 93);
            this.cbDataDownloadEnabled.Name = "cbDataDownloadEnabled";
            this.cbDataDownloadEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbDataDownloadEnabled.TabIndex = 9;
            this.cbDataDownloadEnabled.Text = "Enabled";
            this.cbDataDownloadEnabled.UseVisualStyleBackColor = true;
            // 
            // btnForceDownload
            // 
            this.btnForceDownload.Location = new System.Drawing.Point(17, 64);
            this.btnForceDownload.Name = "btnForceDownload";
            this.btnForceDownload.Size = new System.Drawing.Size(110, 23);
            this.btnForceDownload.TabIndex = 8;
            this.btnForceDownload.Text = "Force download";
            this.btnForceDownload.UseVisualStyleBackColor = true;
            this.btnForceDownload.Click += new System.EventHandler(this.btnForceDownload_Click);
            // 
            // nudDataDownloadInterval
            // 
            this.nudDataDownloadInterval.Location = new System.Drawing.Point(17, 38);
            this.nudDataDownloadInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudDataDownloadInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDataDownloadInterval.Name = "nudDataDownloadInterval";
            this.nudDataDownloadInterval.Size = new System.Drawing.Size(110, 20);
            this.nudDataDownloadInterval.TabIndex = 7;
            this.nudDataDownloadInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDataDownloadInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Interval (in minutes)";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnForcePost);
            this.groupBox7.Controls.Add(this.nudPostOfflineLogsInterval);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Location = new System.Drawing.Point(15, 338);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(140, 106);
            this.groupBox7.TabIndex = 14;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Post Offline Logs";
            // 
            // btnForcePost
            // 
            this.btnForcePost.Location = new System.Drawing.Point(17, 64);
            this.btnForcePost.Name = "btnForcePost";
            this.btnForcePost.Size = new System.Drawing.Size(110, 23);
            this.btnForcePost.TabIndex = 9;
            this.btnForcePost.Text = "Force post";
            this.btnForcePost.UseVisualStyleBackColor = true;
            this.btnForcePost.Click += new System.EventHandler(this.btnForcePost_Click);
            // 
            // nudPostOfflineLogsInterval
            // 
            this.nudPostOfflineLogsInterval.Location = new System.Drawing.Point(17, 38);
            this.nudPostOfflineLogsInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudPostOfflineLogsInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPostOfflineLogsInterval.Name = "nudPostOfflineLogsInterval";
            this.nudPostOfflineLogsInterval.Size = new System.Drawing.Size(110, 20);
            this.nudPostOfflineLogsInterval.TabIndex = 7;
            this.nudPostOfflineLogsInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPostOfflineLogsInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Interval (in minutes)";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cbFullview);
            this.groupBox9.Location = new System.Drawing.Point(15, 450);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(140, 48);
            this.groupBox9.TabIndex = 16;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Application View";
            // 
            // cbFullview
            // 
            this.cbFullview.AutoSize = true;
            this.cbFullview.Location = new System.Drawing.Point(17, 19);
            this.cbFullview.Name = "cbFullview";
            this.cbFullview.Size = new System.Drawing.Size(120, 17);
            this.cbFullview.TabIndex = 6;
            this.cbFullview.Text = "Fullscreen/Topmost";
            this.cbFullview.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.nudMaxLogDisplayTime);
            this.groupBox10.Controls.Add(this.label10);
            this.groupBox10.Location = new System.Drawing.Point(15, 504);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(140, 70);
            this.groupBox10.TabIndex = 17;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Log Display";
            // 
            // nudMaxLogDisplayTime
            // 
            this.nudMaxLogDisplayTime.Location = new System.Drawing.Point(17, 38);
            this.nudMaxLogDisplayTime.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudMaxLogDisplayTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxLogDisplayTime.Name = "nudMaxLogDisplayTime";
            this.nudMaxLogDisplayTime.Size = new System.Drawing.Size(110, 20);
            this.nudMaxLogDisplayTime.TabIndex = 7;
            this.nudMaxLogDisplayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMaxLogDisplayTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Max. Time (in seconds)";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cbEnableLANDevice);
            this.groupBox11.Controls.Add(this.cbEnableUSBDevice);
            this.groupBox11.Controls.Add(this.nudDeviceReconnectInterval);
            this.groupBox11.Controls.Add(this.label11);
            this.groupBox11.Location = new System.Drawing.Point(352, 535);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(188, 111);
            this.groupBox11.TabIndex = 18;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Device Monitor";
            // 
            // cbEnableLANDevice
            // 
            this.cbEnableLANDevice.AutoSize = true;
            this.cbEnableLANDevice.Location = new System.Drawing.Point(17, 87);
            this.cbEnableLANDevice.Name = "cbEnableLANDevice";
            this.cbEnableLANDevice.Size = new System.Drawing.Size(120, 17);
            this.cbEnableLANDevice.TabIndex = 11;
            this.cbEnableLANDevice.Text = "Enable LAN Device";
            this.cbEnableLANDevice.UseVisualStyleBackColor = true;
            // 
            // cbEnableUSBDevice
            // 
            this.cbEnableUSBDevice.AutoSize = true;
            this.cbEnableUSBDevice.Location = new System.Drawing.Point(17, 64);
            this.cbEnableUSBDevice.Name = "cbEnableUSBDevice";
            this.cbEnableUSBDevice.Size = new System.Drawing.Size(121, 17);
            this.cbEnableUSBDevice.TabIndex = 10;
            this.cbEnableUSBDevice.Text = "Enable USB Device";
            this.cbEnableUSBDevice.UseVisualStyleBackColor = true;
            // 
            // nudDeviceReconnectInterval
            // 
            this.nudDeviceReconnectInterval.Location = new System.Drawing.Point(17, 38);
            this.nudDeviceReconnectInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudDeviceReconnectInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeviceReconnectInterval.Name = "nudDeviceReconnectInterval";
            this.nudDeviceReconnectInterval.Size = new System.Drawing.Size(110, 20);
            this.nudDeviceReconnectInterval.TabIndex = 7;
            this.nudDeviceReconnectInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDeviceReconnectInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(161, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Reconnect Inverval (in seconds)";
            // 
            // btnDownloadPhotos
            // 
            this.btnDownloadPhotos.Location = new System.Drawing.Point(17, 181);
            this.btnDownloadPhotos.Name = "btnDownloadPhotos";
            this.btnDownloadPhotos.Size = new System.Drawing.Size(110, 23);
            this.btnDownloadPhotos.TabIndex = 9;
            this.btnDownloadPhotos.Text = "Download";
            this.btnDownloadPhotos.UseVisualStyleBackColor = true;
            this.btnDownloadPhotos.Click += new System.EventHandler(this.btnDownloadPhotos_Click);
            // 
            // lblPhotosVersion
            // 
            this.lblPhotosVersion.AutoSize = true;
            this.lblPhotosVersion.ForeColor = System.Drawing.Color.Blue;
            this.lblPhotosVersion.Location = new System.Drawing.Point(133, 186);
            this.lblPhotosVersion.Name = "lblPhotosVersion";
            this.lblPhotosVersion.Size = new System.Drawing.Size(41, 13);
            this.lblPhotosVersion.TabIndex = 12;
            this.lblPhotosVersion.Text = "[empty]";
            // 
            // btnBrowsePL
            // 
            this.btnBrowsePL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowsePL.Location = new System.Drawing.Point(276, 152);
            this.btnBrowsePL.Name = "btnBrowsePL";
            this.btnBrowsePL.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePL.TabIndex = 14;
            this.btnBrowsePL.Text = "Browse...";
            this.btnBrowsePL.UseVisualStyleBackColor = true;
            this.btnBrowsePL.Click += new System.EventHandler(this.btnBrowsePL_Click);
            // 
            // tbPL
            // 
            this.tbPL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPL.Location = new System.Drawing.Point(17, 155);
            this.tbPL.Name = "tbPL";
            this.tbPL.Size = new System.Drawing.Size(253, 20);
            this.tbPL.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Photos Location";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(552, 693);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnApplyAndClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerTimeQueryInterval)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTerminalId)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataDownloadInterval)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPostOfflineLogsInterval)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLogDisplayTime)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeviceReconnectInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbLongTimeFormat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbAppUrl;
        private System.Windows.Forms.NumericUpDown nudServerTimeQueryInterval;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbOfflineMsg;
        private System.Windows.Forms.Button btnDownloadLogo;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnApplyAndClose;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nudTerminalId;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnBrowseODL;
        private System.Windows.Forms.TextBox tbODL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowseTLL;
        private System.Windows.Forms.TextBox tbTLL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblDSVersion;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nudDataDownloadInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.NumericUpDown nudPostOfflineLogsInterval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnForceDownload;
        private System.Windows.Forms.Button btnForcePost;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cbFullview;
        private System.Windows.Forms.CheckBox cbUseServerTime;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.NumericUpDown nudMaxLogDisplayTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.NumericUpDown nudDeviceReconnectInterval;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbDataDownloadEnabled;
        private System.Windows.Forms.CheckBox cbEnableLANDevice;
        private System.Windows.Forms.CheckBox cbEnableUSBDevice;
        private System.Windows.Forms.Button btnForceQueryTime;
        private System.Windows.Forms.Label lblPhotosVersion;
        private System.Windows.Forms.Button btnDownloadPhotos;
        private System.Windows.Forms.Button btnBrowsePL;
        private System.Windows.Forms.TextBox tbPL;
        private System.Windows.Forms.Label label8;
    }
}