using System.Web;
using System.Web.Optimization;

namespace hnliving.web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-3.3.7").Include(
                      "~/Scripts/bootstrap-3.3.7.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css-3.3.7").Include(
                      "~/Content/bootstrap-3.3.7.css",
                      "~/Content/site.css",
                      "~/Content/animate.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",    // popper.js需要放在bootstrap.js前面
                      "~/Scripts/bootstrap.js",
                      //"~/Scripts/bootstrap.bundle.js",    // 添加后下拉框第一次点击需要点击两次才生效
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/color.css",
                      "~/Content/animate.css"));

            #region Swiper

            bundles.Add(new ScriptBundle("~/bundles/Swiper").Include(
                      "~/Plugins/Swiper/js/swiper.js",
                      "~/Plugins/Swiper/js/swiper.animate.min.js"));

            bundles.Add(new StyleBundle("~/Content/Swiper").Include(
                      "~/Plugins/Swiper/css/swiper.css",
                      "~/Plugins/Swiper/css/animate.css"));


            bundles.Add(new ScriptBundle("~/bundles/Swiper-min").Include(
                      "~/Plugins/Swiper/js/swiper.min.js",
                      "~/Plugins/Swiper/js/swiper.animate.min.js"));

            bundles.Add(new StyleBundle("~/Content/Swiper-min").Include(
                      "~/Plugins/Swiper/css/swiper.min.css",
                      "~/Plugins/Swiper/css/animate.min.css"));
            #endregion

            #region Anime.js
            bundles.Add(new ScriptBundle("~/bundles/anime-js").Include(
                      "~/Plugins/Anime/anime.min.js"));
            #endregion

            #region H5

            bundles.Add(new ScriptBundle("~/bundles/H5").Include(
                      "~/Plugins/Swiper/js/swiper.min.js",
                      "~/Plugins/Swiper/js/swiper.animate.min.js"));

            bundles.Add(new StyleBundle("~/Content/H5").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/color.css",
                      "~/Content/animate.css",
                      "~/Plugins/Swiper/css/swiper.min.css",
                      "~/Plugins/Swiper/css/animate.min.css"));
            #endregion
        }
    }
}