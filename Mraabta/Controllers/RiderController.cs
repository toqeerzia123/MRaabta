using MRaabta.Models;
using MRaabta.Repo;
using System.Web.Mvc;

namespace Mraabta.Controllers
{
    public class RiderController : Controller
    {
        RiderDB rdb = new RiderDB();
        //RiderDB rd;
        //// GET: Rider
        //public RiderController()
        //{
        //    rd = new RiderDB();
        //}
        //public async Task<ActionResult> ViewPickUp()
        //{
        //    var branchcode = Session["BRANCHCODE"].ToString();
        //    await rd.OpenAsync();
        //    var riders = await rd.GetRiders(branchcode);
        //    rd.Close();
        //    ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
        //    return View();
        //}
        public ActionResult GetRiders()
        {

            RiderData rm = new RiderData();
            rm.RiderList = new SelectList(rdb.getRiders(Session["BRANCHCODE"].ToString()), "riderCode", "riderName");
            return View(rm);
        }




        // GET: Rider/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rider/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rider/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rider/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rider/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rider/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
