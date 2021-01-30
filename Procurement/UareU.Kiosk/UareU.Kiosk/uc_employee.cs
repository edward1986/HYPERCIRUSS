using Module.Time;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class uc_employee : UserControl
    {
        public int MaxLogDisplayTime { get; set; }

        int dispCount = 0;

        public uc_employee()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            lblEmployee.Text = "";
            lblOffice.Text = "";
            lblPosition.Text = "";
            pbEmployeePhoto.Image = null;

            lblTimeLog.Text = "";
            lblMode.Text = "";

            tmrDisplay.Enabled = false;
            dispCount = 0;

            uc_schedules1.Clear();
            uc_schedules1.Visible = false;
        }

        public void ShowEmployee(EmployeeModel em, etPeriod p)
        {

            lblEmployee.Text = em.employee.ToUpper();
            lblOffice.Text = em.office;
            lblPosition.Text = em.position;

            if (em.photo == null)
            {
                pbEmployeePhoto.Image = new Bitmap(Properties.Resources.user);
            }
            else
            {
                pbEmployeePhoto.Image = em.photo;
            }
            

            lblTimeLog.Text = FixedSettings.LongTimeFormat ? em.timelog.ToLongTimeString() : em.timelog.ToShortDateString();
            lblMode.Text = em.mode == (int)TimeLogMode.In ? "IN" : "OUT";

            tmrDisplay.Enabled = true;

            uc_schedules1.ShowSchedules(em, p);
            uc_schedules1.Visible = true;
        }

        private void tmrDisplay_Tick(object sender, System.EventArgs e)
        {
            dispCount++;

            if (dispCount >= MaxLogDisplayTime)
            {
                Clear();
            }
        }
    }
}
