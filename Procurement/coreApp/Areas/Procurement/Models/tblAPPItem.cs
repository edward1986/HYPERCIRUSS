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

    public partial class tblAPPItemMetaData
    {
        [Display(Name = "PPMP Number")]
        public string PPMPNumber { get; set; }

        [Required]
        public int AppId { get; set; }

        public int PPMPId { get; set; }

        public int MOP { get; set; }

        [Display(Name = "Advertisement/Posting of IB/REI")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Advertisement { get; set; }

        [Display(Name = "Submission/Opening of Bids")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Submission { get; set; }

        [Display(Name = "Notice Of Award")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NoticeOfAward { get; set; }

        [Display(Name = "Contract Signing")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ContractSigning { get; set; }

        [Display(Name = "Amount")]
        [Range(0, 9999999999, ErrorMessage = "Invalid Total value")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double Total { get; set; }

        [Range(0, 9999999999)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double MOOE { get; set; }

        [Range(0, 9999999999)]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        public double CO { get; set; }

        public string Remarks { get; set; }

    }

    [MetadataType(typeof(tblAPPItemMetaData))]
    public partial class tblAppItem
    {

        public tblPPMP getPPMP
        {
            get
            {
                using (procurementDataContext context = new procurementDataContext())
                {

                    tblPPMP ppmpItem = context.tblPPMPs.Where(x => x.Id == PPMPId).SingleOrDefault();

                    return ppmpItem;
                }
            }

        }

        [Display(Name = "Mode Of Payment")]
        public string MOP_desc
        {
            get
            {
                if (MOP == null)
                {
                    return "";
                }
                else
                {
                    string myText = "";

                    switch (MOP)
                    {
                        case 0:
                            myText = "NP-53.9 - Small Value Procurement";
                            break;
                        case 1:
                            myText = "Shopping";
                            break;
                        case 2:
                            myText = "Limited Source Bidding";
                            break;
                        case 3:
                            myText = "Competitive Bidding";
                            break;
                        case 4:
                            myText = "Direct Contracting";
                            break;
                        case 5:
                            myText = "Repeat Order";
                            break;
                        case 6:
                            myText = "NP-53.1 Two Failed Biddings";
                            break;
                        case 7:
                            myText = "NP-53.2 Emergency Cases";
                            break;
                        case 8:
                            myText = "NP-53.3 Take-Over of Contracts";
                            break;
                        case 9:
                            myText = "NP-53.4 Adjacent or Contiguous";
                            break;
                        case 10:
                            myText = "NP-53.5 Agency-to-Agency";
                            break;
                        case 11:
                            myText = "NP-53.6 Scientific, Scholarly, Artistic Work, Exclusive Technology and Media Services";
                            break;
                        case 12:
                            myText = "NP-53.7 Highly Technical Consultants";
                            break;
                        case 13:
                            myText = "NP-53.8 Defense Cooperation Agreement";
                            break;
                        case 14:
                            myText = "NP-53.10 Lease of Real Property and Venues";
                            break;
                        case 15:
                            myText = "NP-53.11 NGO Participation";
                            break;
                        case 16:
                            myText = "NP-53.12 Community Participation";
                            break;
                        case 17:
                            myText = "NP-53.13 UN Agencies, Int'l Organizations or International Financing Institutions";
                            break;
                        case 18:
                            myText = "Others - Foreign-funded procurement";
                            break;

                    }

                    //return Enum.GetName(typeof(ModeOfProcurement), MOP);
                    return myText;
                }

            }
        }
               
    }
}