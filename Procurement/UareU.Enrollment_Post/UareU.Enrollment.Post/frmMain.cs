using DPUruNet;
using Module.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UareU.Common;

namespace UareU.Enrollment_Post
{

    public partial class frmMain : Form, ParentForm, IForm
    {
        public bool IsOnline { get; set; }

        KioskDataManager dataManager;


        public frmMain()
        {
            InitializeComponent();

            dataManager = new KioskDataManager();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "USB-Fingerprint Enrollment version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (getData(true, true, true))
            {
                frmAuthentication frmAuth = new frmAuthentication(Offline.Data.BaseSettings);

                if (frmAuth.ShowDialog() != DialogResult.OK)
                {
                    frmAuth.Dispose();
                    this.Close();
                }
                else
                {
                    GetReader();
                }
            }
            else
            {
                MessageBox.Show("No data found. Application will exit");

                Common.FoceExit = true;
                Application.Exit();
            }
        }

        public bool getData(bool showDialog, bool forceDownload, bool overWrite)
        {
            frmDownloadDS dl = new frmDownloadDS(this, forceDownload, 
                overWrite,
                Properties.Settings.Default.OfflineDataPath,
                 Properties.Settings.Default.DatasourceFilename,
                Properties.Settings.Default.DataUrl);

            if (showDialog)
            {
                dl.ShowDialog();
            }
            else
            {
                dl.Download(forceDownload, overWrite);
            }

            dataManager.LoadDataFile();

            bool ok = Offline.Data != null;

            if (ok)
            {
                Module.DB.Procs.Common.Global_CachedTables = new cachedTables(Offline.Data);
            }

            return ok;
        }

        #region Employee Search/List
        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                doSearch(tbSearch.Text);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            doSearch(tbSearch.Text);
        }

        private void doSearch(string searchStr)
        {
            List<EmployeeModel> employees = Offline.Data.Employees
                .ToList()
                .Select(x => new EmployeeModel
                {
                    EmployeeId = x.EmployeeId,
                    Employee = x.FullName,
                    FP1 = false,
                    FP2 = false,
                    FP3 = false,
                    FP4 = false,
                    FP5 = false,
                    FP6 = false,
                    FP7 = false,
                    FP8 = false,
                    FP9 = false,
                    FP10 = false
                })
                .Where(x => x.Employee.ToLower().Contains(searchStr.ToLower()))
                .OrderBy(x => x.Employee)
                .ToList();

            employees.ForEach(x =>
            {
                tblEmployee_FP fp = Offline.Data.EmployeeFPs.Where(y => y.EmployeeId == x.EmployeeId).SingleOrDefault();
                if (fp != null)
                {
                    x.FP1 = fp.FP1 != null;
                    x.FP2 = fp.FP2 != null;
                    x.FP3 = fp.FP3 != null;
                    x.FP4 = fp.FP4 != null;
                    x.FP5 = fp.FP5 != null;
                    x.FP6 = fp.FP6 != null;
                    x.FP7 = fp.FP7 != null;
                    x.FP8 = fp.FP8 != null;
                    x.FP9 = fp.FP9 != null;
                    x.FP10 = fp.FP10 != null;
                }
            });

            dgvEmployees.Rows.Clear();
            foreach (var e in employees)
            {
                dgvEmployees.Rows.Add(e.EmployeeId, e.Employee.ToUpper(), e.FP1, e.FP2, e.FP3, e.FP4, e.FP5, e.FP6, e.FP7, e.FP8, e.FP9, e.FP10);
            }
        }

        private void dgvEmployees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentReader == null)
            {
                MessageBox.Show("No reader selected", "Enroll Fingerprint");
                return;
            }

            DataGridView dg = (DataGridView)sender;
            DataGridViewRow row = dg.Rows[e.RowIndex];

            int employeeId = (int)row.Cells[0].Value;
            string employeeName = row.Cells[1].Value.ToString();

            EnrollmentControl enrollmentControl = new EnrollmentControl();
            enrollmentControl._sender = this;
            enrollmentControl.employeeId = employeeId;
            enrollmentControl.employeeName = employeeName;

            enrollmentControl.ShowDialog();

            if (enrollmentControl.fp != null)
            {
                int c = 2;
                row.Cells[c + 0].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP1);
                row.Cells[c + 1].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP2);
                row.Cells[c + 2].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP3);
                row.Cells[c + 3].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP4);
                row.Cells[c + 4].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP5);
                row.Cells[c + 5].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP6);
                row.Cells[c + 6].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP7);
                row.Cells[c + 7].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP8);
                row.Cells[c + 8].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP9);
                row.Cells[c + 9].Value = !string.IsNullOrEmpty(enrollmentControl.fp.FP10);
            }
        }

        #endregion


        #region Device

        private delegate void SendMessageCallback(Action state, object payload);
        private enum Action
        {
            UpdateReaderState
        }

        private Reader currentReader;

        public Reader CurrentReader
        {
            get { return currentReader; }
            set
            {
                currentReader = value;
                SendMessage(Action.UpdateReaderState, value);
            }
        }

        private bool GetReader()
        {
            ReaderCollection _readers = ReaderCollection.GetReaders();
            if (_readers.Any())
            {
                CurrentReader = _readers.First();
                return true;
            }
            else
            {
                CurrentReader = null;
                return false;
            }
        }

        private void btnReaderSelect_Click(System.Object sender, System.EventArgs e)
        {
            ReaderSelection _readerSelection = new ReaderSelection();
            _readerSelection.Sender = this;

            _readerSelection.ShowDialog();

            _readerSelection.Dispose();
            _readerSelection = null;
        }


        private void SendMessage(Action state, object payload)
        {
            if (this.txtReaderSelected.InvokeRequired)
            {
                SendMessageCallback d = new SendMessageCallback(SendMessage);
                this.Invoke(d, new object[] { state, payload });
            }
            else
            {
                switch (state)
                {
                    case Action.UpdateReaderState:
                        if ((Reader)payload != null)
                        {
                            txtReaderSelected.Text = ((Reader)payload).Description.SerialNumber;
                        }
                        else
                        {
                            txtReaderSelected.Text = String.Empty;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        public Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getData(true, true, true);
            btnSearch_Click(sender, e);
        }
    }
}
