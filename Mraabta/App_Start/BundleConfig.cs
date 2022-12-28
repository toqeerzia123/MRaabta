using System.Web.Optimization;

namespace MRaabta.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include("~/Content/faall.css")
                   .Include("~/Content/bootstrap.min.css")
                   .Include("~/Content/jquery-ui.min.css")
                   .Include("~/Content/jquery-ui.theme.min.css")
                   .Include("~/Content/sidebar.css")
                   .Include("~/Content/bootstrap-toggle.min.css")
                   .Include("~/Content/sweetalert2.min.css")
                   .Include("~/Content/bsdp/bootstrap-datepicker3.min.css")
                   .Include("~/Content/select2.min.css")
                   .Include("~/Content/sweetalert.css")
                   );

            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include("~/Scripts/jquery-3.5.1.min.js")
                .Include("~/Scripts/jquery-ui.min.js")
                //.Include("~/Scripts/popper.min.js")
                .Include("~/Scripts/bootstrap.min.js")
                .Include("~/Scripts/sidebar.js")
                .Include("~/Scripts/bootstrap-toggle.min.js")
                .Include("~/Scripts/sweetalert2.min.js")
                .Include("~/Scripts/moment.min.js")
                //.Include("~/Scripts/jquery.mask.min.js")
                .Include("~/Scripts/bsdp/bootstrap-datepicker.min.js")
                .Include("~/Scripts/jquery.zoom.min.js")
                .Include("~/Scripts/select2.min.js")
                .Include("~/Scripts/sweetalert.min.js")
                );
        }
    }
}