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
    public partial class uc_recent_logs : UserControl
    {
        List<EmployeeModel> logs = new List<EmployeeModel>();

        public uc_recent_logs()
        {
            InitializeComponent();
        }
        
        public void Add(EmployeeModel em)
        {
            int limit = 5;

            logs.Add(em);

            if (logs.Count > limit)
            {
                logs.RemoveAt(0);
            }

            ShowLogs();
        }

        private void ShowLogs()
        {
            pnlMain.Controls.Clear();

            for (int i = 0; i < logs.Count; i++)
            {
                EmployeeModel em = logs[logs.Count - 1 - i];

                uc_recent_log_item item = new uc_recent_log_item();
                item.Top = item.Height * i;
                //item.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

                item.ShowEmployee(em);

                pnlMain.Controls.Add(item);
            }

            pnlMain.Refresh();
        }
    }
}
