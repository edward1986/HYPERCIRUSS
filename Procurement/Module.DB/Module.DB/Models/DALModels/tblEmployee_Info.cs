using System.ComponentModel.DataAnnotations;
using System;
using Module.DB.Enums;
using System.IO;

namespace Module.DB
{

    public partial class tblEmployee_InfoMetaData
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
        public string Perm_Address { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, 999999, ErrorMessage = "The field Country (Permanent Address) is required")]
        public int Perm_CountryId { get; set; }

        [Display(Name = "Postal Code")]
        public int Perm_PostalCode { get; set; }

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

        [Display(Name = "Height (m)")]
        public double Height { get; set; }

        [Display(Name = "Weight (kg)")]
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

        [Display(Name = "Dual Citizenship")]
        public bool Citizenship_Dual { get; set; }

        [Display(Name = "By Birth")]
        public bool Citizenship_ByBirth { get; set; }

        [Display(Name = "By Naturalization")]
        public bool Citizenship_ByNaturalization { get; set; }

        [Display(Name = "Details")]
        public string Citizenship_Details { get; set; }

        [Display(Name = "Government Issued ID")]
        public string PDS_GovIssuedId { get; set; }

        [Display(Name = "ID/License/Passport No.")]
        public string PDS_IDLIcensePPNo { get; set; }

        [Display(Name = "Date of Issuance")]
        [DataType(dataType: DataType.Date)]
        public DateTime? PDS_DateOfIssuance { get; set; }

        [Display(Name = "Place of Issuance")]
        public string PDS_PlaceOfIssuance { get; set; }
    }

    [MetadataType(typeof(tblEmployee_InfoMetaData))]
    public partial class tblEmployee_Info
    {
        public tblEmployee_Info(vwEmployee_Info_NoPhoto vw)
            : base()
        {
            EmployeeId = vw.EmployeeId;
            DOB = vw.DOB;
            POB_Address = vw.POB_Address;
            POB_CountryId = vw.POB_CountryId;
            POB_PostalCode = vw.POB_PostalCode;
            Gender = vw.Gender;
            CivilStatus = vw.CivilStatus;
            Nationality_CountryId = vw.Nationality_CountryId;
            TIN = vw.TIN;
            SSS = vw.SSS;
            GSIS = vw.GSIS;
            PAGIBIG = vw.PAGIBIG;
            Home_Address = vw.Home_Address;
            Home_CountryId = vw.Home_CountryId;
            Home_PostalCode = vw.Home_PostalCode;
            Home_TelephoneNo = vw.Home_TelephoneNo;
            MobileNo = vw.MobileNo;
            Height = vw.Height;
            Weight = vw.Weight;
            BloodType = vw.BloodType;
            Spouse_LastName = vw.Spouse_LastName;
            Spouse_FirstName = vw.Spouse_FirstName;
            Spouse_MiddleName = vw.Spouse_MiddleName;
            Spouse_Occupation = vw.Spouse_Occupation;
            Spouse_Employer = vw.Spouse_Employer;
            Spouse_Employer_TelephoneNo = vw.Spouse_Employer_TelephoneNo;
            Father_LastName = vw.Father_LastName;
            Father_FirstName = vw.Father_FirstName;
            Father_MiddleName = vw.Father_MiddleName;
            Mother_LastName = vw.Mother_LastName;
            Mother_FirstName = vw.Mother_FirstName;
            Mother_MiddleName = vw.Mother_MiddleName;
            PHILHEALTH = vw.PHILHEALTH;
            BIRStatus = vw.BIRStatus;
            PDSQ34a = vw.PDSQ34a;
            PDSQ34b = vw.PDSQ34b;
            PDSQ34b_Details = vw.PDSQ34b_Details;
            PDSQ35a = vw.PDSQ35a;
            PDSQ35a_Details = vw.PDSQ35a_Details;
            PDSQ35b = vw.PDSQ35b;
            PDSQ35b_DateFiled = vw.PDSQ35b_DateFiled;
            PDSQ35b_Status = vw.PDSQ35b_Status;
            PDSQ36 = vw.PDSQ36;
            PDSQ36_Details = vw.PDSQ36_Details;
            PDSQ37 = vw.PDSQ37;
            PDSQ37_Details = vw.PDSQ37_Details;
            PDSQ38a = vw.PDSQ38a;
            PDSQ38a_Details = vw.PDSQ38a_Details;
            PDSQ38b = vw.PDSQ38b;
            PDSQ38b_Details = vw.PDSQ38b_Details;
            PDSQ39 = vw.PDSQ39;
            PDSQ39_Details = vw.PDSQ39_Details;
            PDSQ40a = vw.PDSQ40a;
            PDSQ40a_Details = vw.PDSQ40a_Details;
            PDSQ40b = vw.PDSQ40b;
            PDSQ40b_Details = vw.PDSQ40b_Details;
            PDSQ40c = vw.PDSQ40c;
            PDSQ40c_Details = vw.PDSQ40c_Details;
            PDS_GovIssuedId = vw.PDS_GovIssuedId;
            PDS_IDLIcensePPNo = vw.PDS_IDLIcensePPNo;
            PDS_DateOfIssuance = vw.PDS_DateOfIssuance;
            PDS_PlaceOfIssuance = vw.PDS_PlaceOfIssuance;

            Citizenship_Dual = vw.Citizenship_Dual;
            Citizenship_ByBirth = vw.Citizenship_ByBirth;
            Citizenship_ByNaturalization = vw.Citizenship_ByNaturalization;
            Citizenship_Details = vw.Citizenship_Details;

            Perm_Address = vw.Perm_Address;
            Perm_CountryId = vw.Perm_CountryId;
            Perm_PostalCode = vw.Perm_PostalCode;

            if (!string.IsNullOrEmpty(vw.PDSPhotoString))
            {
                PDSPhoto = Convert.FromBase64String(vw.PDSPhotoString);
            }

        }

        public vwEmployee_Info_NoPhoto vw()
        {
            vwEmployee_Info_NoPhoto ret = new vwEmployee_Info_NoPhoto
            {
                EmployeeId = EmployeeId,
                DOB = DOB,
                POB_Address = POB_Address,
                POB_CountryId = POB_CountryId,
                POB_PostalCode = POB_PostalCode,
                Gender = Gender,
                CivilStatus = CivilStatus,
                Nationality_CountryId = Nationality_CountryId,
                TIN = TIN,
                SSS = SSS,
                GSIS = GSIS,
                PAGIBIG = PAGIBIG,
                Home_Address = Home_Address,
                Home_CountryId = Home_CountryId,
                Home_PostalCode = Home_PostalCode,
                Home_TelephoneNo = Home_TelephoneNo,
                MobileNo = MobileNo,
                Height = Height,
                Weight = Weight,
                BloodType = BloodType,
                Spouse_LastName = Spouse_LastName,
                Spouse_FirstName = Spouse_FirstName,
                Spouse_MiddleName = Spouse_MiddleName,
                Spouse_Occupation = Spouse_Occupation,
                Spouse_Employer = Spouse_Employer,
                Spouse_Employer_TelephoneNo = Spouse_Employer_TelephoneNo,
                Father_LastName = Father_LastName,
                Father_FirstName = Father_FirstName,
                Father_MiddleName = Father_MiddleName,
                Mother_LastName = Mother_LastName,
                Mother_FirstName = Mother_FirstName,
                Mother_MiddleName = Mother_MiddleName,
                PHILHEALTH = PHILHEALTH,
                BIRStatus = BIRStatus,
                PDSQ34a = PDSQ34a,
                PDSQ34b = PDSQ34b,
                PDSQ34b_Details = PDSQ34b_Details,
                PDSQ35a = PDSQ35a,
                PDSQ35a_Details = PDSQ35a_Details,
                PDSQ35b = PDSQ35b,
                PDSQ35b_DateFiled = PDSQ35b_DateFiled,
                PDSQ35b_Status = PDSQ35b_Status,
                PDSQ36 = PDSQ36,
                PDSQ36_Details = PDSQ36_Details,
                PDSQ37 = PDSQ37,
                PDSQ37_Details = PDSQ37_Details,
                PDSQ38a = PDSQ38a,
                PDSQ38a_Details = PDSQ38a_Details,
                PDSQ38b = PDSQ38b,
                PDSQ38b_Details = PDSQ38b_Details,
                PDSQ39 = PDSQ39,
                PDSQ39_Details = PDSQ39_Details,
                PDSQ40a = PDSQ40a,
                PDSQ40a_Details = PDSQ40a_Details,
                PDSQ40b = PDSQ40b,
                PDSQ40b_Details = PDSQ40b_Details,
                PDSQ40c = PDSQ40c,
                PDSQ40c_Details = PDSQ40c_Details,
                PDS_GovIssuedId = PDS_GovIssuedId,
                PDS_IDLIcensePPNo = PDS_IDLIcensePPNo,
                PDS_DateOfIssuance = PDS_DateOfIssuance,
                PDS_PlaceOfIssuance = PDS_PlaceOfIssuance,
                Citizenship_Dual = Citizenship_Dual,
                Citizenship_ByBirth = Citizenship_ByBirth,
                Citizenship_ByNaturalization = Citizenship_ByNaturalization,
                Citizenship_Details = Citizenship_Details,
                Perm_Address = Perm_Address,
                Perm_CountryId = Perm_CountryId,
                Perm_PostalCode = Perm_PostalCode
            };

            if (PDSPhoto != null)
            {
                byte[] arr = PDSPhoto.ToArray();
                ret.PDSPhotoString = Convert.ToBase64String(arr);
            }

            return ret;
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

        [Display(Name = "Permanent Address")]
        public string Permanent
        {
            get
            {
                return Module.DB.Procs.Common.getAddress(Perm_Address, Perm_CountryId, Perm_PostalCode);
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