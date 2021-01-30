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
using reports.Aspose;
using Aspose.Words;
using coreApp.Areas.SAM.Enums;

namespace coreApp.Areas.SAM.Controllers
{
    [YearInfoFilter]
    [UserAccessAuthorize(allowedAccess: "sam_transactions")]
    public class PTRController : SAMBaseController, IYearController
    {
        public int Year { get; set; }

        public ActionResult Index()
        {            
            using (samDataContext context = new samDataContext())
            {
                var model = context.tblAREs.Where(x => x.AREDate.Year == Year && x._AREType == (int)AREType.PTR && x.WarehouseId == warehouse.Id).ToList();
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
                tblARE item = context.tblAREs.Where(x => x.Id == id).SingleOrDefault();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_PTR", item);
                }
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.uiIsReadOnly = false;
            ViewBag.uiIncludeId = false;

            tblARE model = new tblARE
            {
                AREDate = DateTime.Today,
                From_Type = (int)iType.Employee,
                To_Type = (int)iType.Employee
            };

            return PartialView("_PTR", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Exclude = "_FromEmployee,_FromContractor,_ToEmployee,_ToContractor")] tblARE model, FormCollection form)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (FixedSettings.IncludeContractors)
            {
                int fromEmployee = form["_FromEmployee"] == "" ? -1 : Convert.ToInt32(form["_FromEmployee"]);
                int fromContractor = form["_FromContractor"] == "" ? -1 : Convert.ToInt32(form["_FromContractor"]);
                int toEmployee = form["_ToEmployee"] == "" ? -1 : Convert.ToInt32(form["_ToEmployee"]);
                int toContractor = form["_ToContractor"] == "" ? -1 : Convert.ToInt32(form["_ToContractor"]);

                model.From_Id = !model.FromContractor ? fromEmployee : fromContractor;
                model.To_Id = !model.ToContractor ? toEmployee : toContractor;
            }
            else
            {
                model.From_Type = (int)iType.Employee;
                model.To_Type = (int)iType.Employee;
            }

            try
            {
                using(samDataContext context = new samDataContext())
                {
                    using (procurementDataContext _context = new procurementDataContext())
                    {
                        if (model.From_Id == -1)
                        {
                            AddError("The From field is required");
                        }

                        if (model.To_Id == -1)
                        {
                            AddError("The To field is required");
                        }

                        if (model.From_Id != -1 && model.To_Id != -1)
                        {
                            if (FixedSettings.IncludeContractors)
                            {
                                if (model.From_Id == model.To_Id)
                                {
                                    AddError("From employee/contractor cannot be the same with To employee/contractor");
                                }
                            }
                            else
                            {
                                if (model.From_Id == model.To_Id)
                                {
                                    AddError("From employee cannot be the same with To employee");
                                }
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            tblARE item = new tblARE
                            {
                                AREDate = model.AREDate,                                
                                From_Id = model.From_Id,
                                To_Id = model.To_Id,
                                From_Type = model.From_Type,
                                To_Type = model.To_Type,
                                FundCluster = model.FundCluster,
                                WarehouseId = warehouse.Id
                            };

                            item.UpdateFields();

                            context.tblAREs.InsertOnSubmit(item);
                            context.SubmitChanges();

                            item.UpdateARENo();

                            res.Remarks = "Record was successfully created";
                        }
                        else
                        {
                            throw new Exception(coreProcs.ShowErrors(ModelState));
                        }
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
                tblARE item = context.tblAREs.Where(x => x.Id == id).Single();
                if (item == null)
                {
                    return new Raw_ActionResult(ModuleConstants.ID_NOT_FOUND);
                }
                else
                {
                    return PartialView("_PTR", item);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "_FromEmployee,_FromContractor,_ToEmployee,_ToContractor")] tblARE model, FormCollection form)
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };

            if (FixedSettings.IncludeContractors)
            {
                int fromEmployee = form["_FromEmployee"] == "" ? -1 : Convert.ToInt32(form["_FromEmployee"]);
                int fromContractor = form["_FromContractor"] == "" ? -1 : Convert.ToInt32(form["_FromContractor"]);
                int toEmployee = form["_ToEmployee"] == "" ? -1 : Convert.ToInt32(form["_ToEmployee"]);
                int toContractor = form["_ToContractor"] == "" ? -1 : Convert.ToInt32(form["_ToContractor"]);

                model.From_Id = !model.FromContractor ? fromEmployee : fromContractor;
                model.To_Id = !model.ToContractor ? toEmployee : toContractor;
            }
            else
            {
                model.From_Type = (int)iType.Employee;
                model.To_Type = (int)iType.Employee;
            }

            try
            {
                using (samDataContext context = new samDataContext())
                {
                    tblARE item = context.tblAREs.Where(x => x.Id == model.Id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }

                    if (model.From_Id == -1)
                    {
                        AddError("The From field is required");
                    }

                    if (model.To_Id == -1)
                    {
                        AddError("The To field is required");
                    }

                    if (model.From_Id != -1 && model.To_Id != -1)
                    {
                        if (FixedSettings.IncludeContractors)
                        {
                            if (model.From_Id == model.To_Id)
                            {
                                AddError("From employee/contractor cannot be the same with To employee/contractor");
                            }
                        }
                        else
                        {
                            if (model.From_Id == model.To_Id)
                            {
                                AddError("From employee cannot be the same with To employee");
                            }
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        item.AREDate = model.AREDate;
                        item.From_Id = model.From_Id;
                        item.To_Id = model.To_Id;
                        item.From_Type = model.From_Type;
                        item.To_Type = model.To_Type;
                        item.FundCluster = model.FundCluster;

                        item.UpdateFields();

                        context.SubmitChanges();

                        item.UpdateARENo();

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
                    tblARE item = context.tblAREs.Where(x => x.Id == id).SingleOrDefault();
                    if (item == null)
                    {
                        throw new Exception(ModuleConstants.ID_NOT_FOUND);
                    }
                    
                    if (ModelState.IsValid)
                    {
                        item.CleanUp();

                        context.tblAREs.DeleteOnSubmit(item);
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
                tblARE aRE = context.tblAREs.Where(x => x.Id == id).SingleOrDefault();
                if (aRE == null)
                {
                    throw new Exception(ModuleConstants.ID_NOT_FOUND);
                }

                string fn = string.Format("ptr-{0}", Year.ToString());

                return new asposeWordsTemplateReport(CustomizeDoc_Aspose).Get("sam-ptr", fn, aRE, dlWord);
            }
        }

        public void CustomizeDoc_Aspose(object data, ref Aspose.Words.Document wordDoc)
        {
            using (samDataContext context = new samDataContext())
            {
                tblARE aRE = (tblARE)data;
                List<tblAREItem> items = context.tblAREItems.Where(x => x.AREId == aRE.Id).ToList();

                tblWarehouse warehouse = DBProcs.get_WarehouseById(aRE.WarehouseId);

                string[] fields = new string[] {
                    "agency", "fund", "ptrno", "from-employee", "to-employee", "to-employee-designation", "date", "warehouse"
                };

                string[] fieldValues = new string[] {
                    FixedSettings.AgencyName.ToUpper(),
                    aRE.FundCluster,
                    aRE.PTRNo,
                    aRE._From_Name,
                    aRE._To_Name,
                    aRE._To_Position,
                    aRE.AREDate.ToShortDateString(),
                    warehouse == null ? "" : warehouse.Description
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
                    tblAREItem e = items[i - 1];
                    
                    string status = "";
                    tblInventoryItem inventory = context.tblInventoryItems.Where(x => x.PropertyNo == e._PropertyNo).SingleOrDefault();
                    if (inventory != null)
                    {
                        status = System.Enum.GetName(typeof(InventoryItemStatus), new InventoryItemModel(inventory).LatestInspection.Status);
                    }

                    if (i > 1)
                    {
                        t.Rows.Add(row.Clone(true));
                    }

                    r = t.Rows[t.Rows.Count - 1];

                    cellIndex = 0;

                    r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._AcquisitionDate.ToShortDateString()));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._PropertyNo));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._Unit));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._ItemName));

                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, e._UnitCost.Value.ToString("#,##0.00")));
                    
                    r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, status));
                }

                t.Rows.Add(row.Clone(true));
                r = t.Rows[t.Rows.Count - 1];

                cellIndex = 0;

                r.Cells[cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, "***NOTHING FOLLOWS***"));
                r.Cells[++cellIndex].FirstParagraph.AppendChild(new Run(wordDoc, ""));

                t.Rows.Add(row.Clone(true));

                t.GetChildNodes(NodeType.Run, true).ToArray().ToList().ForEach(x => ((Run)x).Font.Size = 10);
            }
        }
    }
}