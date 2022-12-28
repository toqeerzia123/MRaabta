using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class DebitNotesController : Controller
    {
        DebitNotesRepo repo = new DebitNotesRepo();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> InsertDebitNote(string ClientID, decimal Amount, string VoucherDate, long Company)
        {
            string Message = "";
            Message = await repo.UpdateDetail(ClientID, Amount, VoucherDate, Company);
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public async Task<JsonResult> UploadBulkDN(List <DebitNotesModel> data)
        {
            string Message = "";
            Message = await repo.UpdateBulkDN(data);
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
}