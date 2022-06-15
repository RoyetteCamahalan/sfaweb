using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace SimpleFFO
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/fnsimple").Include(
                           "~/Scripts/fn.simple.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/vendors/jquery/jquery-1.10.0.min.js",
                            "~/vendors/bootstrap/js/bootstrap.bundle.min.js",
                            "~/vendors/jquery/jquery-ui.min.js",
                            "~/vendors/select2/js/select2.full.min.js",
                            "~/vendors/bs-custom-file-input/bs-custom-file-input.min.js",
                            "~/vendors/moment/moment.min.js",
                            "~/vendors/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                            "~/vendors/inputmask/jquery.inputmask.min.js",
                            "~/vendors/datepicker/daterangepicker.js",
                            "~/vendors/datepicker/custom.js"));

             bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
                            "~/Scripts/adminlte.min.js",
                            "~/Scripts/simple.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                           "~/vendors/dropzone/min/dropzone.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sweetalert").Include(
                           "~/vendors/sweetalert2/sweetalert2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kanban").Include(
                           "~/vendors/ekko-lightbox/ekko-lightbox.min.js",
                           "~/vendors/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                           "~/vendors/filterizr/jquery.filterizr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tables").Include(
                            "~/vendors/datatables/jquery.dataTables.min.js",
                            "~/vendors/datatables-bs4/js/dataTables.bootstrap4.min.js",
                            "~/vendors/datatables-responsive/js/dataTables.responsive.min.js",
                            "~/vendors/datatables-responsive/js/responsive.bootstrap4.min.js",
                            "~/vendors/datatables-buttons/js/dataTables.buttons.min.js",
                            "~/vendors/datatables-buttons/js/buttons.bootstrap4.min.js",
                            "~/vendors/datatables-buttons/js/buttons.html5.min.js",
                            "~/vendors/datatables-bs4/js/dataTables.bootstrap4.min.js"));

            bundles.Add(new StyleBundle("~/Content/maincss").Include(
                            "~/vendors/fontawesome-free/css/all.min.css",
                            "~/vendors/daterangepicker/daterangepicker.css",
                            "~/vendors/select2/css/select2.min.css",
                            "~/vendors/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                            "~/vendors/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
                            "~/vendors/icheck-bootstrap/icheck-bootstrap.min.css",
                            "~/Content/adminlte.min.css",
                            "~/Content/simple.css"));
            
            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                            "~/vendors/dropzone/min/dropzone.min.css"));

            bundles.Add(new StyleBundle("~/Content/kanban").Include(
                            "~/vendors/ekko-lightbox/ekko-lightbox.css",
                            "~/vendors/overlayScrollbars/css/OverlayScrollbars.min.css"));

            bundles.Add(new StyleBundle("~/Content/tablecss").Include(
                            "~/vendors/datatables-bs4/css/dataTables.bootstrap4.min.css",
                            "~/vendors/datatables-responsive/css/responsive.bootstrap4.min.css",
                            "~/vendors/datatables-buttons/css/buttons.bootstrap4.min.css"));
        }
    }
}