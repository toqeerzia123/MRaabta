using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class PickUpRequestController : Controller
    {
        static string brancahCode;
        //brancahCode = Session["BRANCHCODE"].ToString();
        PickupRepo repo;
       
        public PickUpRequestController()
        {
            repo = new PickupRepo();
        }
     
        public async Task<ActionResult> Index()
        {
            try
            {

            var abc = Session["U_ID"].ToString();
            brancahCode = Session["BRANCHCODE"].ToString();
            await repo.OpenAsync();
            var customers = await repo.GetCustomers(brancahCode);
            var couriers = await repo.GetCouriers();
            var priorities = await repo.GetPriorities();
            repo.Close();
            ViewBag.Customers = customers.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewBag.Couriers = couriers.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewBag.Priorities = priorities.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return View();

            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        public async Task<ActionResult> GetLocations(int id)
        {
            try
            {
                await repo.OpenAsync();
                var locations = await repo.GetLocations(id);
                repo.Close();
                return Json(locations, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }


        public async Task<ActionResult> GetRequests(int att)
        {
            try
            {
                await repo.OpenAsync();

                var rs = await repo.GetRequests(att);

                repo.Close();
                return PartialView("_List", rs);
            }
            catch(Exception)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(PickupRequestModel model)
        {
            try
            {
                model.CreatedBy = int.Parse(Session["U_ID"].ToString());
                model.Status = 1;
                model.UserId = int.Parse(Session["U_ID"].ToString());
                model.CreatedOn = DateTime.Now;
                await repo.OpenAsync();
                var rs = await repo.Add(model);
                repo.Close();
                return Json(new { sts = rs ? 1 : 0, msg = rs ? "Save Successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> confirmDelete(int id)
        {
            try
            {
                PickupRequestModel model = new PickupRequestModel();
                model.Id = id;
                model.ModifiedBy = int.Parse(Session["U_ID"].ToString());

                await repo.OpenAsync();
                var rs = await repo.Delete(model);
                repo.Close();
                return Json(new { sts = rs ? 1 : 0, msg = rs ? "Deleted Successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> UpdateRider(int CourierId, int id, int DefaultCourier)
        {
            try
            {
                PickupRequestModel model = new PickupRequestModel();
                model.Id = id;
                model.ModifiedBy = int.Parse(Session["U_ID"].ToString());
                model.CourierId = CourierId;
                model.DefaultCourierId = DefaultCourier;

                await repo.OpenAsync();
                var rs = await repo.Update(model);
                repo.Close();
                return Json(new { sts = rs ? 1 : 0, msg = rs ? "Updated Successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
           
        }


    }
}