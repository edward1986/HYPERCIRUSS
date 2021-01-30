using Module.Device;

namespace UareU.Kiosk
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tmrDateTime = new System.Windows.Forms.Timer(this.components);
            this.tmrData = new System.Windows.Forms.Timer(this.components);
            this.tmrPostOfflineLogs = new System.Windows.Forms.Timer(this.components);
            this.gbDeviceStatus = new System.Windows.Forms.GroupBox();
            this.uc_device_monitor2 = new UareU.Kiosk.uc_device_monitor();
            this.uc_device_monitor1 = new UareU.Kiosk.uc_device_monitor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uc_recent_logs1 = new UareU.Kiosk.uc_recent_logs();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.uc_header1 = new UareU.Kiosk.uc_header();
            this.uc_greeting_and_time1 = new UareU.Kiosk.uc_greeting_and_time();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.uc_timelog_mode1 = new UareU.Kiosk.uc_timelog_mode();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblNoRecordFound = new System.Windows.Forms.Label();
            this.uc_employee1 = new UareU.Kiosk.uc_employee();
            this.uc_device_lan1 = new Module.Device.uc_device_lan();
            this.label1 = new System.Windows.Forms.Label();
            this.uc_device_usb1 = new UareU.Kiosk.uc_device_usb();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uc_announcements1 = new UareU.Kiosk.uc_announcements();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbConfig = new System.Windows.Forms.PictureBox();
            this.gbDeviceStatus.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrDateTime
            // 
            this.tmrDateTime.Tick += new System.EventHandler(this.tmrDateTime_Tick);
            // 
            // tmrData
            // 
            this.tmrData.Tick += new System.EventHandler(this.tmrData_Tick);
            // 
            // tmrPostOfflineLogs
            // 
            this.tmrPostOfflineLogs.Tick += new System.EventHandler(this.tmrPostOfflineLogs_Tick);
            // 
            // gbDeviceStatus
            // 
            this.gbDeviceStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDeviceStatus.BackColor = System.Drawing.Color.Transparent;
            this.gbDeviceStatus.Controls.Add(this.uc_device_monitor2);
            this.gbDeviceStatus.Controls.Add(this.uc_device_monitor1);
            this.gbDeviceStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDeviceStatus.ForeColor = System.Drawing.Color.White;
            this.gbDeviceStatus.Location = new System.Drawing.Point(832, 7);
            this.gbDeviceStatus.Name = "gbDeviceStatus";
            this.gbDeviceStatus.Size = new System.Drawing.Size(373, 104);
            this.gbDeviceStatus.TabIndex = 32;
            this.gbDeviceStatus.TabStop = false;
            this.gbDeviceStatus.Text = "Device Status";
            // 
            // uc_device_monitor2
            // 
            this.uc_device_monitor2.BackColor = System.Drawing.Color.Transparent;
            this.uc_device_monitor2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_device_monitor2.Location = new System.Drawing.Point(184, 26);
            this.uc_device_monitor2.Name = "uc_device_monitor2";
            this.uc_device_monitor2.Size = new System.Drawing.Size(183, 68);
            this.uc_device_monitor2.TabIndex = 1;
            // 
            // uc_device_monitor1
            // 
            this.uc_device_monitor1.BackColor = System.Drawing.Color.Transparent;
            this.uc_device_monitor1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_device_monitor1.Location = new System.Drawing.Point(6, 26);
            this.uc_device_monitor1.Name = "uc_device_monitor1";
            this.uc_device_monitor1.Size = new System.Drawing.Size(172, 68);
            this.uc_device_monitor1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.uc_recent_logs1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(832, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 327);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recent Logs";
            // 
            // uc_recent_logs1
            // 
            this.uc_recent_logs1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_recent_logs1.BackColor = System.Drawing.Color.Transparent;
            this.uc_recent_logs1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_recent_logs1.Location = new System.Drawing.Point(6, 28);
            this.uc_recent_logs1.Name = "uc_recent_logs1";
            this.uc_recent_logs1.Size = new System.Drawing.Size(361, 296);
            this.uc_recent_logs1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.uc_header1);
            this.splitContainer1.Panel1.Controls.Add(this.uc_greeting_and_time1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.gbDeviceStatus);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(1208, 596);
            this.splitContainer1.SplitterDistance = 142;
            this.splitContainer1.TabIndex = 34;
            // 
            // uc_header1
            // 
            this.uc_header1.BackColor = System.Drawing.Color.Transparent;
            this.uc_header1.Location = new System.Drawing.Point(3, 3);
            this.uc_header1.Name = "uc_header1";
            this.uc_header1.Size = new System.Drawing.Size(572, 107);
            this.uc_header1.TabIndex = 27;
            // 
            // uc_greeting_and_time1
            // 
            this.uc_greeting_and_time1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_greeting_and_time1.BackColor = System.Drawing.Color.Transparent;
            this.uc_greeting_and_time1.EnableTimer = false;
            this.uc_greeting_and_time1.Location = new System.Drawing.Point(581, 3);
            this.uc_greeting_and_time1.Name = "uc_greeting_and_time1";
            this.uc_greeting_and_time1.Size = new System.Drawing.Size(624, 139);
            this.uc_greeting_and_time1.TabIndex = 20;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.uc_timelog_mode1);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(490, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(336, 437);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode";
            // 
            // uc_timelog_mode1
            // 
            this.uc_timelog_mode1.BackColor = System.Drawing.Color.Transparent;
            this.uc_timelog_mode1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_timelog_mode1.IsIn = true;
            this.uc_timelog_mode1.Location = new System.Drawing.Point(63, 19);
            this.uc_timelog_mode1.Name = "uc_timelog_mode1";
            this.uc_timelog_mode1.Size = new System.Drawing.Size(217, 447);
            this.uc_timelog_mode1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.lblNoRecordFound);
            this.groupBox3.Controls.Add(this.uc_employee1);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(6, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(478, 437);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log Details";
            // 
            // lblNoRecordFound
            // 
            this.lblNoRecordFound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoRecordFound.BackColor = System.Drawing.Color.Transparent;
            this.lblNoRecordFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoRecordFound.ForeColor = System.Drawing.Color.White;
            this.lblNoRecordFound.Location = new System.Drawing.Point(6, 33);
            this.lblNoRecordFound.Name = "lblNoRecordFound";
            this.lblNoRecordFound.Size = new System.Drawing.Size(466, 27);
            this.lblNoRecordFound.TabIndex = 24;
            this.lblNoRecordFound.Text = "Record not found";
            this.lblNoRecordFound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoRecordFound.Visible = false;
            // 
            // uc_employee1
            // 
            this.uc_employee1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_employee1.BackColor = System.Drawing.Color.Transparent;
            this.uc_employee1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_employee1.Location = new System.Drawing.Point(6, 33);
            this.uc_employee1.MaxLogDisplayTime = 0;
            this.uc_employee1.Name = "uc_employee1";
            this.uc_employee1.Size = new System.Drawing.Size(466, 401);
            this.uc_employee1.TabIndex = 0;
            // 
            // uc_device_lan1
            // 
            this.uc_device_lan1.BackColor = System.Drawing.Color.Transparent;
            this.uc_device_lan1.DeviceIndex = 0;
            this.uc_device_lan1.IP = null;
            this.uc_device_lan1.Location = new System.Drawing.Point(18, 644);
            this.uc_device_lan1.Name = "uc_device_lan1";
            this.uc_device_lan1.OnConnected = null;
            this.uc_device_lan1.OnDisconnected = null;
            this.uc_device_lan1.OnMessage = null;
            this.uc_device_lan1.Port = 0;
            this.uc_device_lan1.Size = new System.Drawing.Size(150, 25);
            this.uc_device_lan1.TabIndex = 21;
            this.uc_device_lan1.ScanComplete += new System.Action<int, int, System.DateTime, int>(this.uc_device_lan1_ScanComplete);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-76, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "label1";
            // 
            // uc_device_usb1
            // 
            this.uc_device_usb1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_device_usb1.BackColor = System.Drawing.Color.Transparent;
            this.uc_device_usb1.Location = new System.Drawing.Point(12, 654);
            this.uc_device_usb1.Name = "uc_device_usb1";
            this.uc_device_usb1.OnConnected = null;
            this.uc_device_usb1.OnDisconnected = null;
            this.uc_device_usb1.OnMessage = null;
            this.uc_device_usb1.Size = new System.Drawing.Size(892, 45);
            this.uc_device_usb1.TabIndex = 11;
            this.uc_device_usb1.ScanComplete += new System.Action<int>(this.uc_device_usb1_ScanComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(18, 614);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(886, 114);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Announcements";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.uc_announcements1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(880, 86);
            this.panel1.TabIndex = 0;
            //
            // uc_announcements1
            // 
            this.uc_announcements1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_announcements1.Location = new System.Drawing.Point(9, -6);
            this.uc_announcements1.Name = "uc_announcements1";
            this.uc_announcements1.Size = new System.Drawing.Size(862, 107);
            this.uc_announcements1.TabIndex = 1;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Location = new System.Drawing.Point(910, 656);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(306, 13);
            this.lblVersion.TabIndex = 45;
            this.lblVersion.Text = "[version]";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(1051, 618);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "You may access your account at:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblInfo.Location = new System.Drawing.Point(913, 638);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(303, 13);
            this.lblInfo.TabIndex = 43;
            this.lblInfo.Text = "[Info]";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbClose
            // 
            this.pbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClose.BackColor = System.Drawing.Color.Transparent;
            this.pbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbClose.Image = ((System.Drawing.Image)(resources.GetObject("pbClose.Image")));
            this.pbClose.Location = new System.Drawing.Point(1163, 695);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(48, 33);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClose.TabIndex = 42;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pbConfig
            // 
            this.pbConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbConfig.BackColor = System.Drawing.Color.Transparent;
            this.pbConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbConfig.Image = ((System.Drawing.Image)(resources.GetObject("pbConfig.Image")));
            this.pbConfig.Location = new System.Drawing.Point(1109, 695);
            this.pbConfig.Name = "pbConfig";
            this.pbConfig.Size = new System.Drawing.Size(48, 33);
            this.pbConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbConfig.TabIndex = 41;
            this.pbConfig.TabStop = false;
            this.pbConfig.Click += new System.EventHandler(this.pbConfig_Click);
            this.pbConfig.MouseEnter += new System.EventHandler(this.pbConfig_MouseEnter);
            this.pbConfig.MouseLeave += new System.EventHandler(this.pbConfig_MouseLeave);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UareU.Kiosk.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(1232, 740);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pbClose);
            this.Controls.Add(this.pbConfig);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.uc_device_lan1);
            this.Controls.Add(this.uc_device_usb1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "SkyHR Attendance Kiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbDeviceStatus.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConfig)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrDateTime;
        private uc_device_usb uc_device_usb1;
        private uc_greeting_and_time uc_greeting_and_time1;
        private System.Windows.Forms.Timer tmrData;
        private System.Windows.Forms.Timer tmrPostOfflineLogs;
        private uc_device_lan uc_device_lan1;
        private uc_header uc_header1;
        private System.Windows.Forms.GroupBox gbDeviceStatus;
        private uc_device_monitor uc_device_monitor2;
        private uc_device_monitor uc_device_monitor1;
        private System.Windows.Forms.GroupBox groupBox2;
        private uc_recent_logs uc_recent_logs1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox4;
        private uc_timelog_mode uc_timelog_mode1;
        private System.Windows.Forms.GroupBox groupBox3;
        private uc_employee uc_employee1;
        private System.Windows.Forms.Label lblNoRecordFound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbConfig;
        private System.Windows.Forms.Panel panel1;
        private uc_announcements uc_announcements1;
    }
}

