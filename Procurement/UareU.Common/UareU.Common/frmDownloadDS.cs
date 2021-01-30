using System;
using System.IO;
using System.Windows.Forms;

namespace UareU.Common
{
    public partial class frmDownloadDS : Form
    {
        IForm form;
        bool forceDownload = false;
        bool overWrite = false;

        string offlineDataPath;
        string dataSourceFilename;
        string dataUrl;

        public string FilePath
        {
            get
            {
                return Path.Combine(offlineDataPath, dataSourceFilename);
            }
        }

        public frmDownloadDS(IForm form, bool forceDownload, bool overWrite, string offlineDataPath, string dataSourceFilename, string dataUrl)
        {
            this.form = form;
            this.forceDownload = forceDownload;
            this.overWrite = overWrite;
            this.offlineDataPath = offlineDataPath;
            this.dataSourceFilename = dataSourceFilename;
            this.dataUrl = dataUrl;

            InitializeComponent();
        }

        public void Download(bool forceDownload, bool overWrite)
        {
            string str = "";
                        
            if (form.IsOnline || forceDownload)
            {
                try
                {
                    str = new coreLib.Objects.WebData().getDataFromUrl(dataUrl);

                    form.IsOnline = true;
                }
                catch
                {
                    form.IsOnline = false;
                }
            }

            if (!string.IsNullOrEmpty(str))
            {

                if (!Directory.Exists(offlineDataPath))
                {
                    Directory.CreateDirectory(offlineDataPath);
                }

                if (!File.Exists(FilePath) || overWrite)
                {
                    using (StreamWriter smw = File.CreateText(FilePath))
                    {
                        smw.Write(str);
                    }
                }
            }

        }

        private void frmDownloading_Shown(object sender, EventArgs e)
        {
            this.Refresh();

            Download(forceDownload, overWrite);

            DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
