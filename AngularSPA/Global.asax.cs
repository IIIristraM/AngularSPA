using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace App.AudioSearcher
{
    public class Application : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
