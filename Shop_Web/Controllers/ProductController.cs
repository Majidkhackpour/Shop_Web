using System.Linq;
using System.Web.Mvc;
using EntityCache.WebBussines;

namespace Shop_Web.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult ShowGroups()
        {
            return PartialView(WebProductGroup.GetAll());
        }
    }
}