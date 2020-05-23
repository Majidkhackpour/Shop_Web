using System.Web.Mvc;
using EntityCache.WebBussines;

namespace Shop_Web.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string q)
        {
            var list = WebProduct.GetAll(q);
            ViewBag.Search = q;
            return View(list);
        }
    }
}