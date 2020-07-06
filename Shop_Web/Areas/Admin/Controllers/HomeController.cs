using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.ViewModels;
using PacketParser.Services;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var visitbus = new VisitBussines();
            var visit = new VisiCountViewModel
            {
                Sum = visitbus.Sum,
                Yesterday = visitbus.Yesterday,
                Today = visitbus.Today,
                Online = System.Web.HttpContext.Current.Application["Online"].ToString().ParseToInt()
            };
            return View(visit);
        }
    }
}