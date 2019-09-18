using System.Web;
using System.Web.Optimization;

namespace MVCCompras
{
  public class BundleConfig
  {
    // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/js/jquery.js",
                  "~/Scripts/js/jquery-ui-1.10.4.min.js",
                  "~/Scripts/js/jquery-1.8.3.min.js",
                  "~/Scripts/js/jquery-ui-1.9.2.custom.min.js"));


      bundles.Add(new ScriptBundle("~/bundles/js").Include(
                 "~/Scripts/js/bootstrap.min.js",
                 "~/Scripts/js/jquery.scrollTo.min.js",
                 "~/Content/assets/jquery-knob/js/jquery.knob.js",
                 "~/Scripts/js/jquery.sparkline.js",
                 "~/Content/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.js",
                 "~/Scripts/js/owl.carousel.js",
                 "~/Scripts/js/fullcalendar.min.js",
                 "~/Content/assets/fullcalendar/fullcalendar/fullcalendar.js",
                 "~/Scripts/js/calendar-custom.js",
                 "~/Scripts/js/jquery.rateit.min.js",
                 "~/Scripts/js/jquery.customSelect.min.js",
                 "~/Content/assets/chart-master/Chart.js",
                 "~/Scripts/js/scripts.js",
                 "~/Scripts/js/sparkline-chart.js",
                 "~/Scripts/js/easy-pie-chart.js",
                 "~/Scripts/js/jquery-jvectormap-1.2.2.min.js",
                 "~/Scripts/js/jquery-jvectormap-world-mill-en.js",
                 "~/Scripts/js/xcharts.min.js",
                 "~/Scripts/js/jquery.autosize.min.js",
                 "~/Scripts/js/jquery.placeholder.min.js",
                 "~/Scripts/js/gdp-data.js",
                 "~/Scripts/js/morris.min.js",
                 "~/Scripts/js/sparklines.js",
                 "~/Scripts/js/charts.js",
                 "~/Scripts/js/jquery.slimscroll.min.js"
                 ));
      //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
      //            "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.validate*"));

      // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
      // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      //sripts para data table
      bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
        "~/Scripts/dataTables.bootstrap4.js",
        "~/Scripts/dataTables.bootstrap4.min.js",
        "~/Scripts/jquery.dataTables.min.js"
        ));

      //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
      //          "~/Scripts/bootstrap.js"));

      //bundles.Add(new StyleBundle("~/Content/css").Include(
      //          "~/Content/bootstrap.css",
      //          "~/Content/site.css"));

      bundles.Add(new StyleBundle("~/Content/Tables").Include(
        "~/Content/dataTables.bootstrap4.css",
        "~/Content/dataTables.bootstrap4.min.css"
        ));

      bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
          "~/Content/css/bootstrap.min.css",
          "~/Content/css/bootstrap-theme.css"));

      bundles.Add(new StyleBundle("~/Content/iconos").Include(
          "~/Content/css/elegant-icons-style.css",
          "~/Content/css/font-awesome.min.css"));

      bundles.Add(new StyleBundle("~/Content/estilos").Include(
         "~/Content/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css",
         "~/Content/assets/fullcalendar/fullcalendar/fullcalendar.css",
         "~/Content/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css",
         "~/Content/css/owl.carousel.css",
         "~/Content/css/jquery-jvectormap-1.2.2.css",
         "~/Content/css/fullcalendar.css",
         "~/Content/css/widgets.css",
         "~/Content/css/style.css",
         "~/Content/css/style-responsive.css",
         "~/Content/css/xcharts.min.css",
         "~/Content/css/jquery-ui-1.10.4.min.css"));



    }
  }
}
