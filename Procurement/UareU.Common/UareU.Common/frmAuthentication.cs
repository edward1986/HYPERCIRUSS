using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UareU.Common
{
    public partial class frmAuthentication : Form
    {
        List<tblSettings_BaseModule> data;

        public string ApplyButtonText
        {
            get
            {
                return btnLogin.Text;
            }
            set
            {
                btnLogin.Text = value;
            }
        }

        public frmAuthentication(List<tblSettings_BaseModule> data)
        {
            InitializeComponent();

            this.data = data;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (checkInputs())
            {
                if (authenticate())
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    showErrors(new List<string>() { "Invalid password" });
                }
            }
        }

        private bool authenticate()
        {
            return tbPassword.Text == data.Where(x => x.Setting == "KioskPassword" && x.Category == "General").Single().SettingValue;
        }

        private bool checkInputs()
        {
            List<string> err = new List<string>();
            
            if (tbPassword.Text == "")
            {
                err.Add("Password required");
            }

            if (err.Any())
            {
                showErrors(err);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void showErrors(List<string> err)
        {
            string s = "";
            string bullet = err.Count > 1 ? "- " : "";

            foreach (string e in err)
            {
                s += (s == "" ? "" : Environment.NewLine) + bullet + e;
            }

            MessageBox.Show(s);
        }
    }
}
