namespace WebUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using DomainModel.Entities;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                null,
                string.Empty,
                new { controller = "Products", action = "List", category = (string)null, page = 1 });

            routes.MapRoute(
                null,
                "Page{page}",
                new { controller = "Products", action = "List", category = (string)null },
                new { page = @"\d+" });

            routes.MapRoute(
                null,
                "{category}",
                new { controller = "Products", action = "List", page = 1 },
                new { page = @"\d+" });

            routes.MapRoute("Default", "{controller}/{action}");
            routes.MapRoute("Default2", "{controller}/{action}/{id}");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory());
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
            Unity.HttpContextLifetimeManager.DisposeAllObjects();
        }
    }
}