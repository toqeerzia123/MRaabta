using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;
using MRaabta.Repo;

namespace MRaabta.Controllers
{
    public class HistoryController : Controller
    {
        HistoryDB rrd;
        public HistoryController()
        {
            rrd = new HistoryDB();
        }
        HistoryDB rrdd = new HistoryDB();

        // GET: History
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetConsignmentRecords(string ConsignmentNumber)
        {
            try
            {
                String branchcode = Session["BRANCHCODE"].ToString();

                var ConsignmentHistoryModel = new ConsignmentHistoryModel();
                List<ConsignmentHistoryModel> lp = new List<ConsignmentHistoryModel>();

                DataTable dt = new DataTable();
                dt = rrd.getRecords(ConsignmentNumber.Trim());
                if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ConsignmentHistoryModel = new ConsignmentHistoryModel
                            {
                                ConsignmentNumber = dt.Rows[i]["ConsignmentNumber"].ToString(),
                                RiderName = dt.Rows[i]["RiderName"].ToString(),
                                RunSheetNumber = dt.Rows[i]["RunSheetNumber"].ToString(),
                                name = dt.Rows[i]["name"].ToString(),
                                CreatedDate = dt.Rows[i]["CreatedDate"].ToString(),
                                CreatedTime = dt.Rows[i]["CreatedTime"].ToString(),
                                picker_name = dt.Rows[i]["picker_name"].ToString()

                            };
                            lp.Add(ConsignmentHistoryModel);
                        }
                    }
                    else
                    {
                        TempData["error"] = "false";
                        ViewBag.Error = TempData["error"];
                    }
                return View(lp);
            }
            catch (Exception er)
            {
                return Redirect("../Login");
            }

        }
    }
}