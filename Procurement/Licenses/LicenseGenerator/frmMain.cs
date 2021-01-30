using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicenseGenerator
{
    public partial class frmMain : Form
    {
        Licenses.License lic = new Licenses.License();

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbOutputPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (tbApplicationName.Text.Trim() == "")
            {
                MessageBox.Show("Application Name is empty");
            }
            else if (tbMotherboardSerialNo.Text.Trim() == "")
            {
                MessageBox.Show("Motherboard Serial No. is empty");
            }
            else if (tbClientName.Text.Trim() == "")
            {
                MessageBox.Show("Client Name is empty");
            }
            else
            {
                string path = tbOutputPath.Text;
                string fullPath = Path.Combine(path, "application.lic");

                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Output Path does not exist");
                }
                else
                {
                    string keyString = lic.getCurrKeyDigest(tbMotherboardSerialNo.Text);

                    Licenses.LicenseInfo info = new Licenses.LicenseInfo
                    {
                        ApplicationName = tbApplicationName.Text,
                        MotherboardSN = tbMotherboardSerialNo.Text,
                        Client = tbClientName.Text,
                        Key = keyString,
                        Unlimited = cbUnlimited.Checked,
                        Expiry = cbUnlimited.Checked ? default(DateTime) : dtpExpiry.Value,
                        ShowExpiry = cbShowExpiry.Checked,
                        AsposeWords = cbAsposeWords.Checked,
                        AsposeBarcode = cbAsposeBarcode.Checked,
                        AsposeCells = cbAsposeCells.Checked,
                        AsposePdf = cbAsposePdf.Checked,
                        Modules_Procurement = cbProcurement.Checked,
                        Modules_SAM = cbSAM.Checked,
                        Modules_PLDT_MP = cbPLDTMP.Checked
                    };

                    string serialized = JsonConvert.SerializeObject(info);
                    string encrypted_info = lic.Encrypt(serialized);

                    string contents = encrypted_info + lic.FileSeparator + lic.Encrypt(new LicenseString().Aspose_Total_Lic);

                    File.WriteAllText(fullPath, contents);

                    MessageBox.Show("File was successfully generated");
                }

            }
        }

        private void cbUnlimited_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpiry.Visible = !cbUnlimited.Checked;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;

                if (File.Exists(path))
                {
                    lic = new Licenses.License
                    {
                        filePath = path
                    };

                    lic.getLicenseKey();

                    tbApplicationName.Text = lic.Info.ApplicationName;
                    tbMotherboardSerialNo.Text = lic.Info.MotherboardSN;
                    tbClientName.Text = lic.Info.Client;

                    if (lic.Info.Expiry != default(DateTime))
                    {
                        dtpExpiry.Value = lic.Info.Expiry;
                    }

                    cbUnlimited.Checked = lic.Info.Unlimited;
                    cbShowExpiry.Checked = lic.Info.ShowExpiry;
                    cbAsposeWords.Checked = lic.Info.AsposeWords;
                    cbAsposeBarcode.Checked = lic.Info.AsposeBarcode;
                    cbAsposeCells.Checked = lic.Info.AsposeCells;
                    cbAsposePdf.Checked = lic.Info.AsposePdf;
                    cbProcurement.Checked = lic.Info.Modules_Procurement;
                    cbSAM.Checked = lic.Info.Modules_SAM;
                    cbPLDTMP.Checked = lic.Info.Modules_PLDT_MP;
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "License Generator (for License ver. " + lic.GetType().Assembly.GetName().Version + ")";
        }
    }
}
