using System;
using System.Linq;
using System.Web.Mvc;
using EntityCache.WebBussines;

namespace Shop_Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Slider()
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var all = WebSlider.GetAll().ToList();
            var list = all.Where(q => q.IsActive).ToList();
            return PartialView(list);
        }
        public ActionResult WonderLayout()
        {
            return PartialView();
        }
        public ActionResult NewPostLayout()
        {
            return PartialView();
        }
    }
}