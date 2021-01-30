using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class uc_recent_log_item : UserControl
    {
        public uc_recent_log_item()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            lblEmployee.Text = "";
            pbEmployeePhoto.Image = null;

            lblTimeLog.Text = "";
            lblMode.Text = "";
        }

        public void ShowEmployee(EmployeeModel em)
        {
            lblEmployee.Text = em.employee.ToUpper();
            
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
        }
    }
}
