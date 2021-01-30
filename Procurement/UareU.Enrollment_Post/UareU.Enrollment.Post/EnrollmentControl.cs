using System;
using System.Windows.Forms;
using DPUruNet;
using UareU.Common;
using System.Collections.Generic;
using System.Linq;
using Module.DB;

namespace UareU.Enrollment_Post
{
    public partial class EnrollmentControl : Form
    {
        /// <summary>
    /// Holds the main form with many functions common to all of SDK actions.
    /// </summary>
        public ParentForm _sender;

        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public tblEmployee_FP fp;

        private DPCtlUruNet.EnrollmentControl _enrollmentControl;

        private Dictionary<int, Fmd> fmds { get; set; } = new Dictionary<int, Fmd>();

        public EnrollmentControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
    /// Initialize the form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
        private void EnrollmentControl_Load(object sender, EventArgs e)
        {

            this.Text = employeeName;

            if (_enrollmentControl != null)
            {
                _enrollmentControl.Reader = _sender.CurrentReader;
            }
            else
            {
                _enrollmentControl = new DPCtlUruNet.EnrollmentControl(_sender.CurrentReader, Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);
                _enrollmentControl.BackColor = System.Drawing.SystemColors.Window;
                _enrollmentControl.Location = new System.Drawing.Point(3, 3);
                _enrollmentControl.Name = "ctlEnrollmentControl";
                _enrollmentControl.Size = new System.Drawing.Size(482, 346);
                _enrollmentControl.TabIndex = 0;
                _enrollmentControl.OnCancel += new DPCtlUruNet.EnrollmentControl.CancelEnrollment(this.enrollment_OnCancel);
                _enrollmentControl.OnCaptured += new DPCtlUruNet.EnrollmentControl.FingerprintCaptured(this.enrollment_OnCaptured);
                _enrollmentControl.OnDelete += new DPCtlUruNet.EnrollmentControl.DeleteEnrollment(this.enrollment_OnDelete);
                _enrollmentControl.OnEnroll += new DPCtlUruNet.EnrollmentControl.FinishEnrollment(this.enrollment_OnEnroll);
                _enrollmentControl.OnStartEnroll += new DPCtlUruNet.EnrollmentControl.StartEnrollment(this.enrollment_OnStartEnroll);                
            }
            
            this.Controls.Add(_enrollmentControl);

            loadState();
        }

        #region Enrollment Control Events
        private void enrollment_OnCancel(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnCancel:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
            }
            else
            {
                SendMessage("OnCancel:  No Reader Connected, finger " + fingerPosition);
            }

            btnCancel.Visible = false;
            btnSave.Visible = true;
        }
        private void enrollment_OnCaptured(DPCtlUruNet.EnrollmentControl enrollmentControl, CaptureResult captureResult, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnCaptured:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition + ", quality " + captureResult.Quality.ToString());
            }
            else
            {
                SendMessage("OnCaptured:  No Reader Connected, finger " + fingerPosition);
            }

            if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                if (_sender.CurrentReader != null)
                {
                    _sender.CurrentReader.Dispose();
                    _sender.CurrentReader = null;
                }

                // Disconnect reader from enrollment control
                _enrollmentControl.Reader = null; 
                                
                MessageBox.Show("Error:  " + captureResult.ResultCode);
                btnCancel.Visible = false;
            }
            else
            {
                if (captureResult.Data != null)
                {
                    foreach (Fid.Fiv fiv in captureResult.Data.Views)
                    {
                        pbFingerprint.Image = _sender.CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height);
                    }
                }
            }
        }
        private void enrollment_OnDelete(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnDelete:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
                SendMessage("Enrolled Finger Mask: " + _enrollmentControl.EnrolledFingerMask);
                //SendMessage("Disabled Finger Mask: " + _enrollmentControl.DisabledFingerMask);
            }
            else
            {
                SendMessage("OnDelete:  No Reader Connected, finger " + fingerPosition);
            }

            fmds.Remove(fingerPosition);

            if (fmds.Count == 0)
            {
                //_sender.btnIdentificationControl.Enabled = false;
            }
        }
        private void enrollment_OnEnroll(DPCtlUruNet.EnrollmentControl enrollmentControl, DataResult<Fmd> result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnEnroll:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
                SendMessage("Enrolled Finger Mask: " + _enrollmentControl.EnrolledFingerMask);
                //SendMessage("Disabled Finger Mask: " + _enrollmentControl.DisabledFingerMask);
            }
            else
            {
                SendMessage("OnEnroll:  No Reader Connected, finger " + fingerPosition);
            }

            if (result != null && result.Data != null)
            {
                fmds.Add(fingerPosition, result.Data);
            }

            btnCancel.Visible = false;
            btnSave.Visible = true;

            //_sender.btnIdentificationControl.Enabled = true;
        }
        private void enrollment_OnStartEnroll(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnStartEnroll:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
            }
            else
            {
                SendMessage("OnStartEnroll:  No Reader Connected, finger " + fingerPosition);
            }

            btnCancel.Visible = true;
            btnSave.Visible = false;
        }
        #endregion

        private void loadState()
        {

            fmds.Clear();

            tblEmployee_FP fp = Offline.Data.EmployeeFPs.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
            if (fp != null)
            {
                int count = 0;
                int mask = 0;

                for (int i = 0; i <= 9; i++)
                {
                    string data = "";
                    if (i == 0) data = fp.FP1;
                    if (i == 1) data = fp.FP2;
                    if (i == 2) data = fp.FP3;
                    if (i == 3) data = fp.FP4;
                    if (i == 4) data = fp.FP5;
                    if (i == 5) data = fp.FP6;
                    if (i == 6) data = fp.FP7;
                    if (i == 7) data = fp.FP8;
                    if (i == 8) data = fp.FP9;
                    if (i == 9) data = fp.FP10;

                    if (!string.IsNullOrEmpty(data))
                    {
                        count += 1;
                        mask += mapFinger(i);
                        fmds.Add(i, Fmd.DeserializeXml(data));
                    }
                }

                if (mask > 0)
                {
                    _enrollmentControl.EnrolledFingerMask = mask;
                }

            }

            _enrollmentControl.MaxEnrollFingerCount = 10;
        }

        private int mapFinger(int fingerPos)
        {
            int ret = 0;

            if (fingerPos == 0) ret = 1;
            if (fingerPos == 1) ret = 2;
            if (fingerPos == 2) ret = 4;
            if (fingerPos == 3) ret = 8;
            if (fingerPos == 4) ret = 16;
            if (fingerPos == 5) ret = 32;
            if (fingerPos == 6) ret = 64;
            if (fingerPos == 7) ret = 128;
            if (fingerPos == 8) ret = 256;
            if (fingerPos == 9) ret = 512;
            

            return ret;
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to cancel this enrollment?", "Are You Sure?", buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _enrollmentControl.Cancel();
            }
        }

        private void saveTemplates()
        {
            tblEmployee_FP fp = Offline.Data.EmployeeFPs.Where(x => x.EmployeeId == employeeId).SingleOrDefault();
            if (fp != null)
            {
                fp.Clear();
            }
            else
            {
                fp = new tblEmployee_FP
                {
                    EmployeeId = employeeId
                };
                
            }

            Fmd data;

            data = fmds.Where(x => x.Key == 0).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP1 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 1).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP2 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 2).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP3 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 3).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP4 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 4).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP5 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 5).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP6 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 6).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP7 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 7).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP8 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 8).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP9 = Fmd.SerializeXml(data);
            data = fmds.Where(x => x.Key == 9).Select(x => x.Value).SingleOrDefault(); if (data != null) fp.FP10 = Fmd.SerializeXml(data);


            for (int i = 0; i <= 9; i++)
            {
                string value = null;

                data = fmds.Where(x => x.Key == i).Select(x => x.Value).SingleOrDefault();
                if (data != null)
                {
                    value = Fmd.SerializeXml(data);
                }

                Common.SaveOnDB(employeeId, i, value);
            }

            this.fp = fp;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveTemplates();
            this.Close();
        }

        private void frmEnrollment_Closed(object sender, EventArgs e)
        {
            _enrollmentControl.Cancel();
        }

        private void SendMessage(string message)
        {
            //txtMessage.Text += message + "\r\n\r\n";
            //txtMessage.SelectionStart = txtMessage.TextLength;
            //txtMessage.ScrollToCaret();
        }
    }
}
