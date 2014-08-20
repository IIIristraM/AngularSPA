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
            bundles.Add(new StyleBundle("~/content/css/app").Include("~/content/app.css"));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/angular").Include(
                "~/scripts/angular.js", 
                "~/scripts/angular-*"));

            bundles.Add(new ScriptBundle("~/js/app").Include(
                "~/scripts/app/filters.js",
                "~/scripts/app/services.js",
                "~/scripts/app/directives.js",
                "~/scripts/app/controllers.js",
                "~/scripts/app/app.js"));
        }
    }
}
