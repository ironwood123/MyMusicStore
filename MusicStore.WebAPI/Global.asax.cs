using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MusicStore.WebAPI.Controllers;
namespace MusicStore.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

          //  Application["UserCount"] = 0;
        }

        //public override void Init()
        //{
        //    this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        //    base.Init();
        //}

        //void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    System.Web.HttpContext.Current.SetSessionStateBehavior(
        //        System.Web.SessionState.SessionStateBehavior.Required);
        //}

        //public void AnonymousIdentification_Creating(Object sender, System.Web.Security.AnonymousIdentificationEventArgs e)
        //{
        //    // Change the anonymous id
        //    e.AnonymousID = "mysite.com_Anonymous_User_" + DateTime.Now.Ticks;

        //    // Increment count of unique anonymous users
        //    Application["UserCount"] = Int32.Parse(Application["UserCount"].ToString()) + 1;
        //}
    }
}
