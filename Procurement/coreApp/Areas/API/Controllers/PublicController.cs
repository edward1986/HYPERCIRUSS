using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;
using coreApp.DAL;
using coreApp.Controllers;
using coreApp;
using coreLib.Objects;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Threading;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Concurrent;

namespace coreApp.Areas.API.Controllers
{

    [KnownDeviceAuthorize]
    public class PublicController : Controller
    {

        public ActionResult TestProcess()
        {
            queryResult res = new queryResult { IsSuccessful = true, Data = null, Err = "", Remarks = "" };
            string eventStreamContentType = "text/event-stream";

            try
            {
                if (ModelState.IsValid)
                {
                    Response.ContentType = eventStreamContentType;

                    ProgressData pd = new ProgressData { Total = 20 };

                    for (int i = 1; i <= pd.Total; i++)
                    {
                        pd.Current = i;
                        string updateData = string.Format("event:update\ndata:{0}\n\n", pd.JSONString());

                        Response.Write(updateData);
                        Response.Flush();

                        System.Threading.Thread.Sleep(1000);
                    }
                }
                else
                {
                    throw new Exception(coreProcs.ShowErrors(ModelState));
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                res.Err = coreProcs.ShowErrors(ex);
            }

            string finalData = string.Format("event:done\ndata:{0}\n\n", JsonConvert.SerializeObject(res));

            return Content(finalData, eventStreamContentType);
        }
    }
}