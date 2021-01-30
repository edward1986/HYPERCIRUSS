using Module.Time;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class uc_schedules : UserControl
    {
        public uc_schedules()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            lblTitle.Visible = false;
            pnlView.Controls.Clear();
        }

        private void AppendText(string s, Color color, Font font)
        {
            Label l = new Label();
            l.Text = s;
            l.ForeColor = color;
            l.Font = font;
            l.Anchor = ((AnchorStyles)((((AnchorStyles.Top) | AnchorStyles.Left) | AnchorStyles.Right)));
            l.MaximumSize = new Size(pnlView.Width, 0);
            l.AutoSize = true;

            addLabel(ref l);
        }

        private void addLabel(ref Label l)
        {
            int c = pnlView.Controls.Count;
            if (c > 0)
            {
                Label last = (Label)pnlView.Controls[c - 1];
                l.Top = last.Top + last.Height + 10;
            }

            pnlView.Controls.Add(l);
            pnlView.Refresh();
        }

        private void AppendTime(string time, string desc, string suffix)
        {
            Label lblTime = new Label();
            lblTime.Text = time;
            lblTime.Font = new Font(pnlView.Font.FontFamily, 12);
            lblTime.AutoSize = true;

            addLabel(ref lblTime);

            string suff = string.IsNullOrEmpty(suffix) ? "" : string.Format("({0})", suffix);

            Label lblDesc = new Label();
            lblDesc.Text = desc + suff;
            lblDesc.Font = new Font(pnlView.Font.FontFamily, 12);
            lblDesc.Top = lblTime.Top;
            lblDesc.Left = lblTime.Left + lblTime.Width + 20;
            lblDesc.MaximumSize = new Size(pnlView.Width - lblDesc.Left, 0);
            lblDesc.AutoSize = true;

            pnlView.Controls.Add(lblDesc);
            pnlView.Refresh();
        }

        public void ShowSchedules(EmployeeModel em, etPeriod p)
        {
            lblTitle.Visible = true;

            pnlView.Controls.Clear();

            etDay d = p.Days.First();

            if (d.IsHoliday)
            {
                string s = string.Format("This day was declared as a holiday: {0}", d.Holiday.Holiday);
                AppendText(s, Color.Blue, new Font(pnlView.Font.FontFamily, 12, FontStyle.Bold));
            }
            
            if (d.NoSchedule)
            {
                lblTitle.Text = "No schedule for today";
            }
            else
            {
                lblTitle.Text = "Schedules for today";
                
                foreach (var time in d.Times)
                {
                    string seg = "";
                    string desc = "";
                    string suff = "";

                    if (time.segment.IsWorkSpan)
                    {
                        seg = time.segment.SegmentType_Desc;
                        desc = time.segment.Description;
                    }
                    else
                    {
                        seg = time.segment.TimeIn.ToString("hh:mm tt") + " - " + time.segment.TimeOut.ToString("hh:mm tt");
                        desc = time.segment.Description;

                        if (time.segment.SegmentType != (int)Module.DB.Enums.SegmentType.Regular)
                        {
                            suff = time.segment.SegmentType_Desc;
                        }
                    }

                    AppendTime(seg, desc, suff);
                }                
            }

            pnlView.Visible = true;
        }
    }
}
