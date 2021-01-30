using coreLib.Encryption;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Licenses
{
    public class LicenseInfo
    {
        public string ApplicationName { get; set; }
        public string Client { get; set; }
        public string MotherboardSN { get; set; }
        public DateTime Expiry { get; set; }
        public bool Unlimited { get; set; }
        public bool ShowExpiry { get; set; }
        public string Key { get; set; }

        public bool AsposeWords { get; set; }
        public bool AsposeCells { get; set; }
        public bool AsposeBarcode { get; set; }
        public bool AsposePdf { get; set; }

        public bool Modules_Procurement { get; set; }
        public bool Modules_SAM { get; set; }
        public bool Modules_PLDT_MP { get; set; }

        public string ExpiryString
        {
            get
            {
                string ret = "";

                if (Unlimited)
                {
                    ret = "[Unlimited]";
                }
                else
                {
                    double daysRemaining = (Expiry - DateTime.Now).TotalDays;
                    ret = string.Format("{0} [{1} days remaining]", Expiry.ToShortDateString(), daysRemaining.ToString("0.#"));
                }

                return ret;
            }
        }
    }

    public class AsposeLicenseInfo
    {
        public string Type { get; set; }
        public bool IsSet { get; set; }
        public string Error { get; set; }
    }
        
    public class License
    {
        private string secretKey = "I'm Mighty";
        private clAsposeLicense AsposeLicense = new clAsposeLicense();
        private List<AsposeLicenseInfo> _asposeLicenseSet = new List<AsposeLicenseInfo>();
        public string FileSeparator = "[========SEPARATOR========]";
        private string licenseString = "";
        public string filePath = "";
        private string applicationName = "";
        private List<string> _validationErrors = new List<string>();

        public string Encrypt(string s)
        {
            return new Sym(this.secretKey).Encrypt(s);
        }

        private string Decrypt(string s)
        {
            return new Sym(this.secretKey).Decrypt(s);
        }

        public AsposeLicenseInfo[] AsposeLicenseSet
        {
            get
            {
                return this._asposeLicenseSet.ToArray();
            }
        }

        public bool HasModule
        {
            get
            {
                return this.Info.Modules_Procurement || this.Info.Modules_SAM || this.Info.Modules_PLDT_MP;
            }
        }

        public LicenseInfo Info { get; set; }

        public bool HasLicenseFile { get; set; }

        public bool CorrectHash { get; set; }

        public bool CorrectApplication { get; set; }

        public bool HasExpired { get; set; }

        public bool IsValid { get; set; }

        public string[] ValidationErrors
        {
            get
            {
                return this._validationErrors.ToArray();
            }
        }

        public License()
        {

        }

        public License(string filePath, string applicationName)
        {
            this.filePath = filePath;
            this.applicationName = applicationName;
            this.Validate();
        }

        public void Validate()
        {
            this.IsValid = false;
            this.CorrectHash = false;
            this.CorrectApplication = false;
            this.HasExpired = false;
            this._validationErrors.Clear();
            try
            {
                string currKeyDigest = this.getCurrKeyDigest("");
                this.getLicenseKey();
                this.CorrectHash = currKeyDigest == this.Info.Key;
                this.CorrectApplication = this.applicationName == this.Info.ApplicationName;
                if (!this.Info.Unlimited)
                    this.HasExpired = false;
                this.IsValid = this.CorrectApplication && this.CorrectHash && !this.HasExpired;
                if (!this.CorrectHash)
                    this._validationErrors.Add("Invalid license file");
                if (this.HasExpired)
                    this._validationErrors.Add("License has expired");
                this.SetAsposeLicenses();
            }
            catch (Exception ex)
            {
                this._validationErrors.Add(ex.Message);
            }
        }

        public string all_space_SN(string _sn, string id)
        {
            string str1 = _sn;
            if (string.IsNullOrEmpty(str1))
                str1 = id;
            if (str1 != "" && str1.Trim() == "")
            {
                string str2 = "";
                for (int index = 1; index <= str1.Length; ++index)
                    str2 += index.ToString();
                str1 = str2;
            }
            return str1;
        }

        private string getMachineNo()
        {
            machine.machine machine = new machine.machine();
            string x = this.all_space_SN(machine.getMotherBoardSerial(), machine.getMachineUniqueID(false));
            return this.all_space_SN(machine.getMotherBoardSerial(), machine.getMachineUniqueID(false));
        }

        public string getCurrKeyDigest(string machineSerialNumber = "")
        {
            if (machineSerialNumber == "")
                machineSerialNumber = this.getMachineNo();
            return this.GetSHA1Digest(string.Format("{0}:{1}", (object)machineSerialNumber, (object)this.secretKey));
        }

        private string GetSHA1Digest(string message)
        {
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(message));
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("X2"));
            return stringBuilder.ToString().ToLower();
        }

        public void getLicenseKey()
        {
            this.Info = new LicenseInfo();
            this.HasLicenseFile = File.Exists(this.filePath);
            if (!this.HasLicenseFile)
                throw new Exception("License file not found");
            string str = File.ReadAllText(this.filePath);
            int length = str.IndexOf(this.FileSeparator);
            string s1 = str.Substring(0, length);
            string s2 = str.Substring(length + this.FileSeparator.Length);
            this.Info = JsonConvert.DeserializeObject<LicenseInfo>(this.Decrypt(s1));
            this.licenseString = this.Decrypt(s2);
        }

        private void SetAsposeLicenses()
        {
            Aspose.Pdf.License license1 = new Aspose.Pdf.License();
            Aspose.Words.License license2 = new Aspose.Words.License();
            Aspose.Cells.License license3 = new Aspose.Cells.License();
            Aspose.BarCode.License license4 = new Aspose.BarCode.License();
            license1.SetLicense("D:\\License.lic");
            license2.SetLicense("D:\\License.lic");
            license3.SetLicense("D:\\License.lic");
            license4.SetLicense("D:\\License.lic");
        }

        private bool SetAsposeLicense(AsposeLicenseType LicenseType)
        {
            bool flag = false;
            string name = Enum.GetName(typeof(AsposeLicenseType), (object)LicenseType);
            AsposeLicenseInfo asposeLicenseInfo = new AsposeLicenseInfo()
            {
                Type = name,
                IsSet = false,
                Error = ""
            };
            try
            {
                this.AsposeLicense.SetAsposeLicense(LicenseType, this.licenseString);
                asposeLicenseInfo.IsSet = true;
                flag = true;
            }
            catch (Exception ex)
            {
                asposeLicenseInfo.Error = ex.Message;
            }
            this._asposeLicenseSet.Add(asposeLicenseInfo);
            return flag;
        }
    }

class clAsposeLicense
    {
        
        public void SetAsposeLicense(AsposeLicenseType LicenseType, string LicenseString)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {

                    StreamWriter sw = new StreamWriter(stream);
                    sw.Write(LicenseString);
                    sw.Flush();
                    stream.Position = 0;

                    switch (LicenseType)
                    {
                        case AsposeLicenseType.Aspose_Words:
                            new Aspose.Words.License().SetLicense(stream);
                            break;
                        case AsposeLicenseType.Aspose_Cells:
                            new Aspose.Cells.License().SetLicense(stream);
                            break;
                        case AsposeLicenseType.Aspose_Barcode:
                            new Aspose.BarCode.License().SetLicense(stream);
                            break;
                        case AsposeLicenseType.Aspose_Pdf:
                            new Aspose.Pdf.License().SetLicense(stream);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("There was an error setting the license: " + e.Message);
            }
        }
    }

    public enum AsposeLicenseType
    {
        Aspose_Words = 0,
        Aspose_Cells = 1,
        Aspose_Barcode = 2,
        Aspose_Pdf = 3
    }
}
