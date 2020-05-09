using System.Web.Mvc;

namespace Shop_Web.Controllers
{
    public class _ManageEmailsController : Controller
    {

        public ActionResult ActivationEmail()
        {
            return PartialView();
        }
    }
}