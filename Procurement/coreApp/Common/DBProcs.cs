using coreApp.Areas.Procurement;
using coreApp.Areas.Procurement.DAL;
using coreApp.Areas.SAM;
using coreApp.DAL;
using coreLib.Extensions;
using Microsoft.AspNet.Identity;
using Module.Core;
using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace coreApp
{
    public static class DBProcs
    {       
        public static tblWarehouse get_Warehouse(tblEmployee employee)
        {
            return get_WarehouseById(employee.WarehouseId);
        }

        public static tblWarehouse get_WarehouseById(int? id)
        {
            using (samDataContext context = new samDataContext())
            {
                return context.tblWarehouses.Where(x => x.Id == id).SingleOrDefault();
            }
        }

        public static void save_EmployeeInfo(tblEmployee_Info model)
        {
            using (dalDataContext context = new dalDataContext())
            {
                List<tblEmployee_Info> infos = context.tblEmployee_Infos.Where(x => x.InfoId == model.InfoId).ToList();

                if (infos.Count > 1)
                {
                    tblEmployee_Info newInfo = new tblEmployee_Info
                    {
                        EmployeeId = model.EmployeeId
                    };

                    context.tblEmployee_Infos.InsertOnSubmit(newInfo);
                    context.tblEmployee_Infos.DeleteAllOnSubmit(infos);
                    context.SubmitChanges();
                }


                tblEmployee_Info Info = context.tblEmployee_Infos.Where(x => x.InfoId == model.InfoId).SingleOrDefault();
                if (Info == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    Info.EmployeeId = model.EmployeeId;
                    Info.DOB = model.DOB;
                    Info.POB_Address = model.POB_Address;
                    Info.POB_CountryId = model.POB_CountryId;
                    Info.POB_PostalCode = model.POB_PostalCode;
                    Info.Gender = model.Gender;
                    Info.CivilStatus = model.CivilStatus;
                    Info.Nationality_CountryId = model.Nationality_CountryId;
                    Info.Citizenship_Dual = model.Citizenship_Dual;
                    Info.Citizenship_ByBirth = model.Citizenship_ByBirth;
                    Info.Citizenship_ByNaturalization = model.Citizenship_ByNaturalization;
                    Info.Citizenship_Details = model.Citizenship_Details;
                    Info.BIRStatus = model.BIRStatus;
                    Info.TIN = model.TIN;
                    Info.SSS = model.SSS;
                    Info.GSIS = model.GSIS;
                    Info.PAGIBIG = model.PAGIBIG;
                    Info.PHILHEALTH = model.PHILHEALTH;
                    Info.Perm_Address = model.Perm_Address;
                    Info.Perm_CountryId = model.Perm_CountryId;
                    Info.Perm_PostalCode = model.Perm_PostalCode;
                    Info.Home_Address = model.Home_Address;
                    Info.Home_CountryId = model.Home_CountryId;
                    Info.Home_PostalCode = model.Home_PostalCode;
                    Info.Home_TelephoneNo = model.Home_TelephoneNo;
                    Info.MobileNo = model.MobileNo;
                    Info.Height = model.Height;
                    Info.Weight = model.Weight;
                    Info.BloodType = model.BloodType;
                    Info.Spouse_LastName = model.Spouse_LastName;
                    Info.Spouse_FirstName = model.Spouse_FirstName;
                    Info.Spouse_MiddleName = model.Spouse_MiddleName;
                    Info.Spouse_Occupation = model.Spouse_Occupation;
                    Info.Spouse_Employer = model.Spouse_Employer;
                    Info.Spouse_Employer_TelephoneNo = model.Spouse_Employer_TelephoneNo;
                    Info.Father_LastName = model.Father_LastName;
                    Info.Father_FirstName = model.Father_FirstName;
                    Info.Father_MiddleName = model.Father_MiddleName;
                    Info.Mother_LastName = model.Mother_LastName;
                    Info.Mother_FirstName = model.Mother_FirstName;
                    Info.Mother_MiddleName = model.Mother_MiddleName;

                    Info.PDS_GovIssuedId = model.PDS_GovIssuedId;
                    Info.PDS_IDLIcensePPNo = model.PDS_IDLIcensePPNo;
                    Info.PDS_DateOfIssuance = model.PDS_DateOfIssuance;
                    Info.PDS_PlaceOfIssuance = model.PDS_PlaceOfIssuance;

                    context.SubmitChanges();
                }
            }
        }

        public static void save_EmployeeOtherInfo(tblEmployee_OtherInfo model)
        {
            using (hr2017DataContext context = new hr2017DataContext())
            {
                tblEmployee_OtherInfo Info = context.tblEmployee_OtherInfos.Where(x => x.Id == model.Id).SingleOrDefault();
                if (Info == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    Info.EmployeeId = model.EmployeeId;
                    Info.Skills = model.Skills;
                    Info.NonAcademic = model.NonAcademic;
                    Info.Membership = model.Membership;

                    context.SubmitChanges();
                }
            }
        }


        public static void upload_EmployeePhotos(tblEmployee employee, HttpPostedFileBase ProfilePhoto, string ProfilePhoto_Remove, HttpPostedFileBase PDSPhoto, string PDSPhoto_Remove, ref List<string> msg, ref List<string> err)
        {
            bool removeProfilePhoto = ProfilePhoto_Remove == "True";
            bool removePDSPhoto = PDSPhoto_Remove == "True";
            
            if (removeProfilePhoto)
            {
                using (dalDataContext context = new dalDataContext())
                {
                    AspNetUsers_Photo user = context.AspNetUsers_Photos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                    if (user == null)
                    {
                        err.Add("No Profile photo to remove");
                    }
                    else
                    {
                        context.AspNetUsers_Photos.DeleteOnSubmit(user);
                        context.SubmitChanges();

                        msg.Add("Profile photo was successfully removed");
                    }
                }
            }
            else if (ProfilePhoto != null)
            {
                bool proceed = true;

                if (!FixedSettings.PhotoFileTypes.Split(',').Contains(ProfilePhoto.ContentType))
                {
                    err.Add("Invalid file type");
                    proceed = false;
                }

                if (FixedSettings.PhotoFileSize < ProfilePhoto.ContentLength)
                {
                    err.Add("File size exceeded file-size limit (" + FixedSettings.PhotoFileSize.ToBytes() + ")");
                    proceed = false;
                }

                if (proceed)
                {

                    try
                    {
                        using (dalDataContext context = new dalDataContext())
                        {
                            AspNetUsers_Photo user = context.AspNetUsers_Photos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                            if (user == null)
                            {
                                if (employee.UserId == null)
                                {
                                    throw new Exception("Cannot upload profile photo. Employee has no login account");
                                }
                                else
                                {
                                    user = new AspNetUsers_Photo { EmployeeId = employee.EmployeeId };
                                    context.AspNetUsers_Photos.InsertOnSubmit(user);
                                }
                            }

                            byte[] imgByte = new Byte[ProfilePhoto.ContentLength];
                            ProfilePhoto.InputStream.Read(imgByte, 0, ProfilePhoto.ContentLength);

                            user.Photo = imgByte;

                            context.SubmitChanges();

                            msg.Add("Profile photo was successfully uploaded");
                        }
                    }
                    catch (Exception ex)
                    {
                        err.Add(coreProcs.ShowErrors(ex));
                    }
                }
            }

            if ((removePDSPhoto || (!removePDSPhoto && PDSPhoto != null)) && !Cache.Get().userAccess.HasPermission())
            {
                err.Add(ModuleConstants_Authorization.USERACCESS_UNAUTHORIZED_ACTION);
            }
            else if (removePDSPhoto)
            {
                using (dalDataContext context = new dalDataContext())
                {
                    tblEmployee_Info info = context.tblEmployee_Infos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                    if (info == null)
                    {
                        err.Add("No PDS photo to remove");
                    }
                    else if (info.PDSPhoto == null)
                    {
                        err.Add("No PDS photo to remove");
                    }
                    else
                    {
                        info.PDSPhoto = null;
                        context.SubmitChanges();

                        msg.Add("PDS photo was successfully removed");
                    }
                }
            }
            else if (PDSPhoto != null)
            {
                bool proceed = true;

                if (!FixedSettings.PhotoFileTypes.Split(',').Contains(PDSPhoto.ContentType))
                {
                    err.Add("Invalid file type");
                    proceed = false;
                }

                if (FixedSettings.PhotoFileSize < PDSPhoto.ContentLength)
                {
                    err.Add("File size exceeded file-size limit (" + FixedSettings.PhotoFileSize.ToBytes() + ")");
                    proceed = false;
                }

                if (proceed)
                {

                    try
                    {
                        using (dalDataContext context = new dalDataContext())
                        {
                            tblEmployee_Info info = context.tblEmployee_Infos.Where(x => x.EmployeeId == employee.EmployeeId).SingleOrDefault();
                            if (info == null)
                            {
                                info = new tblEmployee_Info { EmployeeId = employee.EmployeeId };
                                context.tblEmployee_Infos.InsertOnSubmit(info);
                            }

                            byte[] imgByte = new Byte[PDSPhoto.ContentLength];
                            PDSPhoto.InputStream.Read(imgByte, 0, PDSPhoto.ContentLength);

                            info.PDSPhoto = imgByte;

                            context.SubmitChanges();

                            msg.Add("PDS photo was successfully uploaded");
                        }
                    }
                    catch (Exception ex)
                    {
                        err.Add(coreProcs.ShowErrors(ex));
                    }
                }
            }
        }
    }
}