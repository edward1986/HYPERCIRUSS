using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module.DB;

namespace UareU.Kiosk
{
    public partial class uc_announcements : UserControl
    {
        int currentIndex = 0;
        List<tblBulletinBoard> data = new List<tblBulletinBoard>();

        TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel htmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();

        public uc_announcements()
        {
            InitializeComponent();
        }

        private void uc_announcements_Load(object sender, EventArgs e)
        {
            htmlPanel.Dock = DockStyle.Fill;
            Controls.Add(htmlPanel);
        }

        public void Init(List<tblBulletinBoard> data, int cycleInterval = 30000)
        {
            DateTime n = DateTime.Now;
            this.data = data.Where(x => x.Enabled && x.ShowInKiosk && x.DateOfPosting <= n && x.EndOfPosting > n).OrderBy(x => x.DateOfPosting).ThenBy(x => x.DateCreated).ToList();

            tmrCycle.Interval = cycleInterval;
            tmrCycle.Enabled = true;

            show();
        }

        private void tmrCycle_Tick(object sender, EventArgs e)
        {
            show();
        }

        private void show()
        {
            if (data.Any())
            {
                if (currentIndex >= data.Count)
                {
                    currentIndex = 0;
                }

                tblBulletinBoard item = data[currentIndex];
                showItem(item);

                currentIndex++;
            }
            else
            {
                showItem(null);
            }
        }

        private void showItem(tblBulletinBoard item)
        {
            htmlPanel.Text = "";

            if (item != null)
            {
                htmlPanel.Text += "<style>h4 { margin-top:5px;margin-bottom:0px } p { margin:0px 0px }</style>";
                htmlPanel.Text += string.Format("<h4>{0}</h4>", item.Title);
                htmlPanel.Text += item.Contents;
            }
        }
    }
}
