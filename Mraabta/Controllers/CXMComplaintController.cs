using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class CXMComplaintController : Controller
    {
        CXMComplaintDB repo = new CXMComplaintDB();
        public ActionResult Index()
        {
            if (Session["BRANCHCODE"] != null)
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                return View();
            }
            else
            {
                return Redirect("~/Login");
            }
        }

        public ActionResult GetConsignmentDetails(string consignmentNumber)
        {
            CXMConsignment response = new CXMConsignment();
            consignmentNumber = consignmentNumber.Trim();
            try
            {
                response = repo.GetConsignmentDetails(consignmentNumber);

            }
            catch (Exception er)
            {
                response.isSuccess = false;
            }

            return Json(new { response = response }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestType()
        {
            List<string[]> result = new List<string[]>();
            try
            {
                DataTable resp = repo.GetFixedFieldsCXMComplaint();
                for (int j = 0; j < resp.Rows.Count; j++)
                {
                    string[] Type = { "", "" };
                    Type[0] = resp.Rows[j]["rt_ID"].ToString();
                    Type[1] = resp.Rows[j]["rt_name"].ToString();
                    result.Add(Type);
                }


                return Json(new { response = result.ToArray() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Clear();
                return Json(new { response = result.ToArray() }, JsonRequestBehavior.AllowGet);
            }
            finally
            { }
        }

        public ActionResult GetStandardNotes(string requestTypeValue)
        {
            List<string[]> result = new List<string[]>();
            try
            {
                DataTable response = repo.GetCXMStandardNotes(requestTypeValue);

                for (int j = 0; j < response.Rows.Count; j++)
                {
                    string[] Type = { "", "" };
                    Type[0] = response.Rows[j]["note_ID"].ToString();
                    Type[1] = response.Rows[j]["note_name"].ToString();
                    result.Add(Type);
                }
                return Json(new { response = result.ToArray() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Clear();
                return Json(new { response = result.ToArray() }, JsonRequestBehavior.AllowGet);
            }
            finally
            { }
        }

        public ActionResult SaveComplaint(CXMComplaintConsignmentDetails Data)
        {
             try
            {
                Data.ConsignmentNumber = Data.ConsignmentNumber.Trim();
                Response_CXMComplaintSave resp = repo.CXMComplaint_SaveToDataBase(Data);
                 
                return Json(new { response = resp }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                Response_CXMComplaintSave resp = new Response_CXMComplaintSave();
                resp.isSuccess = false;
                resp.message = "Error saving complaint, " + er.Message.ToString();
                return Json(new { response = resp }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}