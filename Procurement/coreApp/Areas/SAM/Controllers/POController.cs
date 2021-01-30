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
using Module.Core;
using coreApp.Areas.Procurement.Filters;
using coreApp.Areas.Procurement.Interfaces;
using Aspose.Words;
using reports.Aspose;
using Module.DB;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_receiving")]
    public class POController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {
            using (samDataContext context = new samDataContext())
            {
                var model = context.tblPOs.Where(x => x.PODate.Year == Year).ToList();
                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = true;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblPO item = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_PO", item);
                }
            }
        }

        public ActionResult Create()
        {

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblPO model = new tblPO
            {
                PODate = DateTime.Today,
                OfficeId = -1
            };

            return PartialView("_PO", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblPO model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            //if (model.OfficeId != -1)
            //{
            //    string seelctedOffice = model.OfficeId.ToString();

            //    model.OfficeId = Convert.ToInt32(seelctedOffice.Split(',')[1]);
            //}

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    if (!string.IsNullOrEmpty(model.PONo))
                    {
                        if (Common.POExists(model.PONo))
                        {
                            AddError("P.O. No. already exists");
                        }
                        else if (Procurement.Common.SupplierPOExists(model.PONo))
                        {
                            AddError("P.O. No. already exists in Procurement");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        /*bool isExistUser = false;
                        
                        using (dalDataContext dataContext = new dalDataContext())
                        {
                            tblEmployee user = dataContext.tblEmployees.Where(x => x.EmployeeId == Convert.ToInt32(model.CreatedBy_UserId)).SingleOrDefault();
                            if (user != null)
                            {
                                isExistUser = true;
                            }
                            else
                            {
                                isExistUser = false;
                            }*/
                        
                        
                        tblPO item = new tblPO
                        {
                            CreatedBy_UserId = Cache.Get().userAccess.user.Id, //isExistUser ? user.UserId : Cache.Get().userAccess.user.Id,
                            PONo = model.PONo,
                            PODate = model.PODate,
                            SupplierId = model.SupplierId,
                            Procurement_POId = model.Procurement_POId,
                            Form_PlaceOfDelivery = model.Form_PlaceOfDelivery,
                            Form_DateOfDelivery = model.Form_DateOfDelivery,
                            Form_PaymentTerm = model.Form_PaymentTerm,
                            Form_DeliveryTerm = model.Form_DeliveryTerm,
                            PRNo = model.PRNo,
                            PRDate = model.PRDate,
                            MOP = model.MOP,
                            WarehouseId = warehouse.Id,
                            OfficeId = model.OfficeId
                        };

                        item.UpdateFields();

                        context.tblPOs.InsertOnSubmit(item);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully created";
                        
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new Raw_ActionResult(ModuleConstants.BAD_REQUEST);
            }

            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            using (samDataContext context = new samDataContext())
            {
                tblPO item = context.tblPOs.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_PO", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPO model)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblPO item = context.tblPOs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (item.Locked)
                    {
                        AddError("P.O. cannot be modified");
                    }

                    if (item.IsWarehouseLocked(warehouse.Id))
                    {
                        AddError("P.O. cannot be modified. It was created in another warehouse");
                    }

                    if (ModelState.IsValid)
                    {
                        item.PODate = model.PODate;
                        item.SupplierId = model.SupplierId;
                        item.Procurement_POId = model.Procurement_POId;
                        item.Form_PlaceOfDelivery = model.Form_PlaceOfDelivery;
                        item.Form_DateOfDelivery = model.Form_DateOfDelivery;
                        item.Form_PaymentTerm = model.Form_PaymentTerm;
                        item.Form_DeliveryTerm = model.Form_DeliveryTerm;
                        item.PRNo = model.PRNo;
                        item.PRDate = model.PRDate;
                        item.MOP = model.MOP;
                        item.OfficeId = model.OfficeId;

                        item.UpdateFields();

                        context.SubmitChanges();

                        res.Remarks = "Record was successfully updated";
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }

            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblPO item = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    POModel pOModel = new POModel(item);

                    if (pOModel.ReceiptItems.Any())
                    {
                        AddError("Cannot delete this item. Receipts have already been made.");
                    }

                    if (item.IsWarehouseLocked(warehouse.Id))
                    {
                        AddError("Cannot delete this item. It was created in another warehouse");
                    }

                    if (ModelState.IsValid)
                    {
                        context.tblPOs.DeleteOnSubmit(item);
                        context.SubmitChanges();

                        res.Remarks = "Record was successfully deleted";
                    }
                    else
                    {
                        throw new Exception(coreProcs.ShowErrors(ModelState));
                    }
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            return Json(res);
        }

        public ActionResult Print(int id, bool dlWord = false)
        {
            using (samDataContext context = new samDataContext())
            {
                tblPO pO = context.tblPOs.Where(x => x.Id == id).SingleOrDefault();
                if (pO == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("po-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-po", fn, pO, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                tblPO pO = (tblPO)data;
                List<tblPOItem> items = context.tblPOItems.Where(x => x.POId == pO.Id).ToList();

                tblWarehouse warehouse = DBProcs.get_WarehouseById(pO.WarehouseId);

                string[] fields = new string[] {
                    "agency", "agency-address", "supplier", "address", "tin", "pono", "date", "mop", "placeofdelivery", "dateofdelivery", "deliveryterm", "paymentterm", "warehouse", "office"
                };

                string[] fieldValues = new string[] {
                    FixedSettings.AgencyName,
                    FixedSettings.AgencyAddress,
                    pO._SupplierName,
                    pO._SupplierAddress,
                    pO._SupplierTIN,
                    pO.PONo,
                    pO.PODate.ToShortDateString(),
                    pO.MOP,
                    pO.Form_PlaceOfDelivery,
                    pO.Form_DateOfDelivery == null ? "" : pO.Form_DateOfDelivery.Value.ToShortDateString(),
                    pO.Form_DeliveryTerm,
                    pO.Form_PaymentTerm,
                    warehouse == null ? "" : warehouse.Description,
                    pO.Office_Desc
                };

                wordDoc.MailMerge.Execute(fields, fieldValues);

                Aspose.Words.Bookmark bookmark = wordDoc.Range.Bookmarks["tableRef"];
                Node _t = bookmark.BookmarkStart.GetAncestor(NodeType.Table);

                Aspose.Words.Tables.Table t = (Aspose.Words.Tables.Table)_t;

                Aspose.Words.Tables.Row r = null;
                Aspose.Words.Tables.Row row = (Aspose.Words.Tables.Row)t.Rows[t.Rows.Count - 1].Clone(true);

                int cellIndex;

                for (int i = 1; i <= items.Count; i++)
                {
                    tblPOItem e = items[i - 1];

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, i.ToString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._Unit));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.ItemName));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Qty.ToString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.UnitCost.ToString("#,##0.00")));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e.Amount.ToString("#,##0.00")));
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "***NOTHING FOLLOWS***"));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                t.Rows.Add(row.Clone(true));

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "TOTAL"));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, items.Sum(x => x.Amount).ToString("#,##0.00")));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}