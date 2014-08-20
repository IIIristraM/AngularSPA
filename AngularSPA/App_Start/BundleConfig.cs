// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.AudioSearcher
{
    using System.Web;
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/css/app").Include("~/content/app.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/angular").Include(
                "~/scripts/angular.js",
                "~/scripts/angular-route.js",
                "~/scripts/angular-animate.js",
                "~/scripts/angular-cookies.js",
                "~/scripts/angular-mocks.js",
                "~/scripts/angular-resource.js",
                "~/scripts/angular-sanitize.js",
                "~/scripts/angular-scenario.js",
                "~/scripts/angular-touch.js",
                "~/scripts/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/js/app").Include(
                "~/scripts/app/filters.js",
                "~/scripts/app/services.js",
                "~/scripts/app/directives.js",
                "~/scripts/app/controllers.js",
                "~/scripts/app/app.js"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
