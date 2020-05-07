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
    }
}