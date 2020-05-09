using System.Web.Mvc;

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
            return PartialView();
        }
        public ActionResult WonderLayout()
        {
            return PartialView();
        }
        public ActionResult New()
        {
            return PartialView();
        }
        public ActionResult PorForoosh()
        {
            return PartialView();
        }
        public ActionResult RightSide()
        {
            return PartialView("RightSide");
        }
    }
}