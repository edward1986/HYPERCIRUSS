using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace device_zks_s5_ocx_wrapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool ConnectTcpip(int n, string ip, int port, int pw)
        {
            return axSB100BPC1.ConnectTcpip(n, ref ip, port, pw);
        }

        public void Disconnect()
        {
            axSB100BPC1.Disconnect();
        }
    }
}
