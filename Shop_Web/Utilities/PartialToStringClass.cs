using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PacketParser.Services;

namespace Shop_Web.Utilities
{
    public static class PartialToStringClass
    {
        public static string RenderPartialView(string controllerName, string partialView, object model)
        {
            try
            {
                var context = new HttpContextWrapper(HttpContext.Current) as HttpContextBase;

                var routes = new RouteData();
                routes.Values.Add("controller", controllerName);

                var requestContext = new RequestContext(context, routes);

                string requiredString = requestContext.RouteData.GetRequiredString("controller");
                var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                var controller = controllerFactory.CreateController(requestContext, requiredString) as ControllerBase;

                controller.ControllerContext = new ControllerContext(context, routes, controller);

                var ViewData = new ViewDataDictionary();

                var TempData = new TempDataDictionary();

                ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialView);
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, ViewData, TempData, sw);

                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return "";
            }
        }

    }
}