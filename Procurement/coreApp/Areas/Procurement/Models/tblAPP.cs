using coreApp.Areas.Procurement.Enums;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;
using Module.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace coreApp.Areas.Procurement.DAL
{

    public partial class tblAPPMetaData
    {
        [Required]
        public string Description { get; set; }

        [Range(0, 99999, ErrorMessage = "The Year field is required")]
        public int Year { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy_UserId { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime SubmitDate { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy_UserId { get; set; }

        [Display(Name = "Date Approved")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime ApproveDate { get; set; }

        [Display(Name = "Returned By")]
        public string ReturnedBy_UserId { get; set; }

        [Display(Name = "Date Returned")]
        [DataType(dataType: DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy h:mm tt}")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Return Message")]
        public string ReturnMessage { get; set; }
    }

    [MetadataType(typeof(tblAPPMetaData))]
    public partial class tblAPP
    {
        public APPModel Model()
        {
            return new APPModel(Id);
        }

        [Display(Name = "Submitted By")]
        public string SubmittedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(SubmittedBy_UserId);
            }
        }

        [Display(Name = "Returned By")]
        public string ReturnedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(ReturnedBy_UserId);
            }
        }

        [Display(Name = "Approved By")]
        public string ApprovedBy
        {
            get
            {
                return Module.DB.Procs.Common.getUser(ApprovedBy_UserId);
            }
        }

        [Display(Name = "Status")]
        public string DocStatus_Desc
        {
            get
            {
                return System.Enum.GetName(typeof(Status), DocStatus);
            }
        }

        public int DocStatus
        {
            get
            {
                int ret = (int)Status.Draft;

                if (ApproveDate != null)
                {
                    ret = (int)Status.Approved;
                }
                else if (ReturnDate != null)
                {
                    if (SubmitDate != null)
                    {
                        if (ReturnDate > SubmitDate)
                        {
                            ret = (int)Status.Returned;
                        }
                        else
                        {
                            ret = (int)Status.ReSubmitted;
                        }
                    }
                }
                else if (SubmitDate != null)
                {
                    ret = (int)Status.Submitted;
                }

                return ret;
            }
        }

        public bool HasBeenSubmitted
        {
            get
            {
                return DocStatus > (int)Status.Returned;
            }
        }

        public bool HasBeenServed
        {
            get
            {
                return IsApproved;
            }
        }

        public bool IsReturned
        {
            get
            {
                return DocStatus == (int)Status.Returned;
            }
        }

        public bool IsApproved
        {
            get
            {
                return DocStatus == (int)Status.Approved;
            }
        }

        public List<tblPPMP> ConsolidatedPPMPs()
        {
            List<tblPPMP> ret = new List<tblPPMP>();
            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(PPMPIds))
                {
                    int[] ids = PPMPIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblPPMPs.Where(x => ids.Contains(x.Id)).ToList());
                }
            }
            return ret;
        }

        public List<tblAppItem> AppItems
        {

            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    List<tblAppItem> items = context.tblAppItems.Where(x => x.AppId == Id).ToList();
                    return items;

                }
            }


        }

        public bool ContainsPPMP(int ppmpId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(PPMPIds))
            {
                ret = PPMPIds.Split(',').Contains(ppmpId.ToString());
            }

            return ret;
        }

        public List<int> ContainedPPMPIds(List<int> ppmpIds)
        {
            List<int> ret = new List<int>();

            if (!string.IsNullOrEmpty(PPMPIds))
            {
                List<int> myList = PPMPIds.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (int x in myList)
                {
                    if (ppmpIds.Contains(x))
                    {
                        ret.Add(x);
                    }
                }
            }

            return ret;
        }

        public bool ContainsCategory(int categoryId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(CategoryIds))
            {
                ret = CategoryIds.Split(',').Contains(categoryId.ToString());
            }

            return ret;
        }

        public bool ContainsInDBM(int inDBMValue)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(InDBMs))
            {
                ret = InDBMs.Split(',').Contains(inDBMValue.ToString());
            }

            return ret;
        }

        public bool ContainsMonth(int month)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(Months))
            {
                ret = Months.Split(',').Contains(month.ToString());
            }

            return ret;
        }

        public List<tblCategory> Categories()
        {
            List<tblCategory> ret = new List<tblCategory>();

            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(CategoryIds))
                {
                    int[] ids = CategoryIds.Split(',').Select(x => int.Parse(x)).ToArray();
                    ret.AddRange(context.tblCategories.Where(x => ids.Contains(x.Id)).ToList());
                }
            }

            return ret;
        }

        public List<SelectListItem> GetMonths()
        {
            List<SelectListItem> ret = new List<SelectListItem>();

            using (procurementDataContext context = new procurementDataContext())
            {
                if (!string.IsNullOrEmpty(Months))
                {
                    int[] m = Months.Split(',').Select(x => int.Parse(x)).ToArray();

                    ret = SelectItems.getMonths(showBlankItem: false).Where(x => m.Contains(int.Parse(x.Value))).ToList();
                }
            }

            return ret;
        }

        public List<tblAPR> ReferringAPRs()
        {
            List<tblAPR> ret = new List<tblAPR>();

            using (procurementDataContext context = new procurementDataContext())
            {
                ret.AddRange(context.tblAPRs.Where(x => x.Year == Year).ToList().Where(x => x.ContainsAPP(Id)));
            }

            return ret;
        }

        public void RemovePPMPId(int ppmpId)
        {
            if (string.IsNullOrEmpty(PPMPIds)) return;

            using (procurementDataContext context = new procurementDataContext())
            {
                List<int> ids = PPMPIds.Split(',').Select(x => int.Parse(x)).ToList();
                int? id = ids.Where(x => x == ppmpId).SingleOrDefault();

                if (id != null)
                {
                    ids.Remove(id.Value);
                }

                var app = context.tblAPPs.Where(x => x.Id == Id).Single();
                app.PPMPIds = string.Join(",", ids);

                context.SubmitChanges();

                new APPModel(Id).SaveItems();
            }
        }

        public bool HasBeenIncludedInSubmittedAPR(int? excludeAPRId = null)
        {
            using (procurementDataContext context = new procurementDataContext())
            {
                bool ret = context.tblAPRs
                    .Where(x => x.Year == Year && (excludeAPRId == null || x.Id != excludeAPRId))
                    .ToList()
                    .Where(x => x.HasBeenSubmitted)
                    .Any(x => x.APPIds.Split(',').Select(y => int.Parse(y)).Contains(Id));

                return ret;
            }
        }
    }
}