using System.Web.Http.Filters;
using System.Web.Mvc;

namespace MRaabta.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PageFilterAttribute());
        }
    }
}
