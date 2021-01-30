using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.Areas.Procurement.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using System.Collections.Generic;
using coreApp.Areas.Procurement.Models;
using coreApp.DAL;

namespace coreApp.Areas.Procurement.Controllers
{
    [UserAccessAuthorize(allowedAccess: "procurement_settings")]
    public class UtilityController : ProcurementBaseController
    {
        public ActionResult RecalculatePPMPs(int year)
        {
            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    foreach (tblPPMP ppmp in context.tblPPMPs.Where(x => x.Year == year))
                    {
                        ppmp._TotalAmount_InDBM = ppmp.Model().TotalAmount(true);
                        ppmp._TotalAmount_NotInDBM = ppmp.Model().TotalAmount(false);
                    }

                    context.SubmitChanges();
                }

                return new Raw_ActionResult("success");
            }
            catch (Exception e)
            {
                return new Raw_ActionResult(e.Message);
            }
        }

        public ActionResult RecalculateAPPs(int year)
        {
            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    foreach (tblAPP app in context.tblAPPs.Where(x => x.Year == year))
                    {
                        APPModel appModel = app.Model();
                        List<tblPPMPItem> consolidated = appModel.ConsolidatedItems();
                        consolidated.ForEach(x => x.APPId = app.Id);

                        List<tblPPMPItem> existing = context.tblPPMPItems
                            .Where(x => x.APPId == app.Id)
                            .ToList();
                        
                        context.tblPPMPItems.DeleteAllOnSubmit(existing);
                        context.tblPPMPItems.InsertAllOnSubmit(consolidated);

                        app._TotalAmount = consolidated.Sum(x => x.Amount);

                        context.SubmitChanges();
                    }
                }

                return new Raw_ActionResult("success");
            }
            catch (Exception e)
            {
                return new Raw_ActionResult(e.Message);
            }
        }

        public ActionResult RecalculatePRs(int year)
        {
            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    foreach (tblPR pr in context.tblPRs.Where(x => x.Year == year))
                    {
                        PRModel prModel = new PRModel(pr.Id);

                        int[] ids = pr.PPMPIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] category_ids = pr.CategoryIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] period_ids = pr.Months.Split(',').Select(x => int.Parse(x)).ToArray();

                        prModel.ImportItems(ids, category_ids, period_ids);
                    }
                }

                return new Raw_ActionResult("success");
            }
            catch (Exception e)
            {
                return new Raw_ActionResult(e.Message);
            }
        }

        public ActionResult RecalculateCPRs(int year)
        {
            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    foreach (tblConsolidatedPR pr in context.tblConsolidatedPRs.Where(x => x.Year == year))
                    {
                        CPRModel prModel = new CPRModel(pr.Id);

                        int[] ids = pr.PRIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] cids = pr.CompanyPRIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] category_ids = pr.CategoryIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] period_ids = pr.Months.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] fund_ids = pr.FundIds.Split(',').Select(x => int.Parse(x)).ToArray();

                        prModel.ImportItems(ids, cids, category_ids, period_ids, fund_ids);
                    }
                }

                return new Raw_ActionResult("success");
            }
            catch (Exception e)
            {
                return new Raw_ActionResult(e.Message);
            }
        }

        public ActionResult RecalculateAPRs(int year)
        {
            try
            {
                using (procurementDataContext context = new procurementDataContext())
                {
                    foreach (tblAPR apr in context.tblAPRs.Where(x => x.Year == year))
                    {
                        APRModel aprModel = new APRModel(apr.Id);

                        int[] ids = apr.APPIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] category_ids = apr.CategoryIds.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] period_ids = apr.Months.Split(',').Select(x => int.Parse(x)).ToArray();
                        int[] fund_ids = apr.FundIds.Split(',').Select(x => int.Parse(x)).ToArray();

                        aprModel.ImportItems(ids, category_ids, period_ids, fund_ids);
                    }
                }

                return new Raw_ActionResult("success");
            }
            catch (Exception e)
            {
                return new Raw_ActionResult(e.Message);
            }
        }
    }
}