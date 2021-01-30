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
    public partial class uc_greeting_and_time : UserControl
    {
        public bool EnableTimer
        {
            get
            {
                return timer1.Enabled;
            }
            set
            {
                timer1.Enabled = value;
            }
        }

        public uc_greeting_and_time()
        {
            InitializeComponent();
        }

        public void SetWarning(bool IsOnline)
        {
            lblOfflineMsg.Text = FixedSettings.OfflineMsg;

            if (IsOnline)
            {
                lblStatus.Text = "ONLINE";
                lblStatus.ForeColor = Color.Lime;
            }
            else
            {
                lblStatus.Text = "OFFLINE";
                lblStatus.ForeColor = Color.Red;
            }

            lblOfflineMsg.Visible = !IsOnline;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frmMain form = (frmMain)this.ParentForm;

            if (form._officialTime == null)
            {
                lblDate.Text = "";
                lblTime.Text = "";
            }
            else
            {
                DateTime v;

                if (form.IsOnline && FixedSettings.UseServerTime)
                {
                    v = form._officialTime.Value;
                    v = v.AddSeconds(1);
                }
                else
                {
                    v = DateTime.Now;
                }                

                form._officialTime = v;
                
                lblDate.Text = v.ToLongDateString();
                lblTime.Text = FixedSettings.LongTimeFormat ? v.ToLongTimeString() : v.ToShortDateString();
            }
            
        }
    }
}
