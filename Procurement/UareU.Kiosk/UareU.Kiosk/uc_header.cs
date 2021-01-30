using System.Windows.Forms;

namespace UareU.Kiosk
{
    public partial class uc_header : UserControl
    {
        public uc_header()
        {
            InitializeComponent();
        }

        public void ShowHeader()
        {
            pbLogo.Image = FixedSettings.Logo;
            lblAgency.Text = FixedSettings.AgencyName;
            lblAddress.Text = FixedSettings.AgencyAddress;
        }
    }
}
