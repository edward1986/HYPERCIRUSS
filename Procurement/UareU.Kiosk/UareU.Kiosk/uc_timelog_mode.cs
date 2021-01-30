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
    public partial class uc_timelog_mode : UserControl
    {
        public bool IsIn { get; set; }

        public TimeLogMode Mode
        {
            get
            {
                return IsIn ? TimeLogMode.In : TimeLogMode.Out;
            }
        }

        public uc_timelog_mode()
        {
            InitializeComponent();
        }

        private void uc_timelog_mode_Load(object sender, EventArgs e)
        {
            IsIn = true;
            SetUI();
        }

        private void SetUI()
        {
            if (IsIn)
            {
                pbIn.Image = imageList1.Images[1];
                pbOut.Image = imageList1.Images[2];
            }
            else
            {
                pbIn.Image = imageList1.Images[0];
                pbOut.Image = imageList1.Images[3];
            }
        }

        private void pbIn_Click(object sender, EventArgs e)
        {
            IsIn = true;
            SetUI();
        }

        private void pbOut_Click(object sender, EventArgs e)
        {
            IsIn = false;
            SetUI();
        }
    }
}
