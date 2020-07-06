using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Http;
using EntityCache.Bussines;
using PacketParser.Services;

namespace Shop_Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            EntityCache.Assistence.ClsCache.Init();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HttpContext.Current.Application["Online"] = 0;
        }
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        protected void Session_Start()
        {
            var online = HttpContext.Current.Application["Online"].ToString().ParseToInt();
            online += 1;
            HttpContext.Current.Application["Online"] = online;
            var ip = Request.UserHostAddress;
            var visit = new VisitBussines()
            {
                Guid = Guid.NewGuid(),
                Modified = DateTime.Now,
                Date = Calendar.MiladiToShamsi(DateTime.Now),
                IP = ip
            };
            visit.Save();
        }

        protected void Session_End()
        {
            var online = HttpContext.Current.Application["Online"].ToString().ParseToInt();
            online -= 1;
            HttpContext.Current.Application["Online"] = online;
        }
    }
}