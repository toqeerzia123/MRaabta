using MRaabta.App_Start;
using MRaabta.Repo;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class DlvRouteController : Controller
    {
        DlvRouteRepo repo;
        public DlvRouteController()
        {
            repo = new DlvRouteRepo();
        }

        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index(bool isCN, string rs = null, string cn = null)
        {
            await repo.OpenAsync();
            var data = await repo.GetRouteByRider(isCN, rs, cn);
            return View(data);
        }
    }

}