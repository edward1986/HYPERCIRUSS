using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace device_zks_s5_app
{
    public partial class frmDownloadData : Form
    {
        public frmDownloadData()
        {
            InitializeComponent();
        }

        private void frmDownloadData_Load(object sender, EventArgs e)
        {
            DateTime n = DateTime.Now;

            initYear(n.Year);
            initMonth(n.Month);
        }

        private void initYear(int defaultYear)
        {
            for (int i = 2015; i <= 2030; i++)
            {
                comboBoxYear.Items.Add(i);
            }

            comboBoxYear.SelectedIndex = comboBoxYear.FindStringExact(defaultYear.ToString());
        }

        private void initMonth(int defaultMonth)
        {
            for (int i = 1; i <= 12; i++)
            {
                comboBoxMonth.Items.Add(new DateTime(2000, i, 1).ToString("MMM"));
            }

            comboBoxMonth.SelectedIndex = defaultMonth - 1;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {

        }

       
    }
}
