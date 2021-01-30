using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class frmDownloadPhotos : Form
    {
        List<string> userIds = new List<string>();
                
        public frmDownloadPhotos(List<string> userIds)
        {
            this.userIds = userIds;

            InitializeComponent();
        }

        private void frmDownloadPhotos_Load(object sender, EventArgs e)
        {
            
        }

        private void Download()
        {
            string photosPath = Properties.Settings.Default.PhotosPath;

            clearPhotosFolder(photosPath);

            string url = Common.GetPhotosUrl();

            int count = userIds.Count;
            int n = 0;

            showProgress(n, count);

            using (WebClient client = new WebClient())
            {
                foreach (string userId in userIds)
                {
                    n++;
                    showProgress(n, count);

                    if (string.IsNullOrEmpty(userId)) continue;

                    try
                    {
                        string fn = Path.Combine(photosPath, userId + ".jpg");
                        string empUrl = url + "?userId=" + userId;

                        using (Stream stream = client.OpenRead(empUrl))
                        {
                            Bitmap bitmap = new Bitmap(stream);
                            if (bitmap != null)
                            {
                                bitmap.Save(fn, ImageFormat.Png);
                            }
                        }
                    }
                    catch (Exception)
                    {
                                                
                    }
                    
                }
            }

            Properties.Settings.Default.PhotosVersion = DateTime.Now.ToString();
            
            this.Close();
        }

        private void clearPhotosFolder(string photosPath)
        {
            DirectoryInfo di = new DirectoryInfo(photosPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch { }                
            }

            foreach (DirectoryInfo sub in di.GetDirectories())
            {
                try
                {
                    sub.Delete(true);
                }
                catch { }
            }
        }

        private void showProgress(int n, int count)
        {
            lblProgress.Text = string.Format("{0}/{1}", n, count);
            this.Refresh();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            Download();
        }
    }
}
