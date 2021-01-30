using System.ComponentModel.DataAnnotations;
using System;
using Module.DB.Enums;

namespace Module.DB
{

    public partial class vwEmployee_InfoMetaData
    {
        [Display(Name = "Id")]
        public int InfoId { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(dataType: DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name = "Address")]
        public string POB_Address { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, 999999, ErrorMessage = "The field Country (Place of Birth) is required")]
        public int POB_CountryId { get; set; }

        [Display(Name = "Postal Code")]
        public int POB_PostalCode { get; set; }

        [Display(Name = "Civil Status")]
        public int CivilStatus { get; set; }

        [Display(Name = "Nationality")]
        public int Nationality_CountryId { get; set; }

        [Display(Name = "Address")]
        public string Home_Address { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, 999999, ErrorMessage = "The field Country (Home) is required")]
        public int Home_CountryId { get; set; }

        [Display(Name = "Postal Code")]
        public int Home_PostalCode { get; set; }

        [Display(Name = "Telephone No.")]
        public string Home_TelephoneNo { get; set; }

        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Display(Name = "Height (cm.)")]
        public double Height { get; set; }

        [Display(Name = "Weight (lbs.)")]
        public double Weight { get; set; }

        [Display(Name = "Blood Type")]
        public string BloodType { get; set; }

        [Display(Name = "Last Name")]
        public string Spouse_LastName { get; set; }

        [Display(Name = "First Name")]
        public string Spouse_FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string Spouse_MiddleName { get; set; }

        [Display(Name = "Occupation")]
        public string Spouse_Occupation { get; set; }

        [Display(Name = "Employer/Bus. Name")]
        public string Spouse_Employer { get; set; }

        [Display(Name = "Employer/Bus. Tel. No.")]
        public string Spouse_Employer_TelephoneNo { get; set; }

        [Display(Name = "Last Name")]
        public string Father_LastName { get; set; }

        [Display(Name = "First Name")]
        public string Father_FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string Father_MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string Mother_LastName { get; set; }

        [Display(Name = "First Name")]
        public string Mother_FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string Mother_MiddleName { get; set; }

        [Required]
        //[Range(0, 99999, ErrorMessage = "The BIR Status field is required")]
        [Display(Name = "BIR Status")]
        public string BIRStatus { get; set; }
    }

    [MetadataType(typeof(vwEmployee_InfoMetaData))]
    public partial class vwEmployee_Info_NoPhoto
    {
        public string PDSPhotoString { get; set; }

        public byte[] PDSPhoto
        {
            get
            {
                if (string.IsNullOrEmpty(PDSPhotoString))
                {
                    return null;
                }
                else
                {
                    return Convert.FromBase64String(PDSPhotoString);
                }
            }
        }

        public Nullable<int> Age
        {
            get
            {
                if (DOB == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now;
                    int age = now.Year - DOB.Value.Year;
                    if (now < DOB.Value.AddYears(age))
                    {
                        age--;
                    }

                    return age;
                }
            }
        }

        [Display(Name = "Place of Birth")]
        public string POB
        {
            get
            {
                return Module.DB.Procs.Common.getAddress(POB_Address, POB_CountryId, POB_PostalCode);
            }
        }

        [Display(Name = "Gender")]
        public string Gender_Desc
        {
            get
            {
                return Gender == null ? "" : System.Enum.GetName(typeof(Gender), Gender);
            }
        }

        [Display(Name = "Civil Status")]
        public string CivilStatus_Desc
        {
            get
            {
                return CivilStatus == null ? "" : System.Enum.GetName(typeof(CivilStatus), CivilStatus);
            }
        }

        [Display(Name = "Blood Type")]
        public string BloodType_Desc
        {
            get
            {
                return BloodType == null ? "" : System.Enum.GetName(typeof(BloodType), BloodType);
            }
        }

        [Display(Name = "Nationality")]
        public string Nationality
        {
            get
            {
                return Nationality_CountryId == null || Nationality_CountryId == -1 ? "" : Procs.Common.getCountry(Nationality_CountryId.Value).Nationality;
            }
        }

        [Display(Name = "Country")]
        public string Country
        {
            get
            {
                return Nationality_CountryId == null || Nationality_CountryId == -1 ? "" : Procs.Common.getCountry(Nationality_CountryId.Value).Name;
            }
        }

        [Display(Name = "Home Address")]
        public string Home
        {
            get
            {
                return Module.DB.Procs.Common.getAddress(Home_Address, Home_CountryId, Home_PostalCode);
            }
        }

        [Display(Name = "Spouse")]
        public string Spouse
        {
            get
            {
                return coreLib.Procs.getFullName_FN(Spouse_LastName, Spouse_FirstName, Spouse_MiddleName, "");
            }
        }

        [Display(Name = "Father")]
        public string Father
        {
            get
            {
                return coreLib.Procs.getFullName_FN(Father_LastName, Father_FirstName, Father_MiddleName, "");
            }
        }

        [Display(Name = "Mother")]
        public string Mother
        {
            get
            {
                return coreLib.Procs.getFullName_FN(Mother_LastName, Mother_FirstName, Mother_MiddleName, "");
            }
        }
    }
}