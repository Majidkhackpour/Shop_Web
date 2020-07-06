using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.WebBussines;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebOpinionsController : Controller
    {

        // GET: Admin/WebOpinions
        public ActionResult Index()
        {
            return View(WebOpinion.GetAll().ToList());
        }

        // GET: Admin/WebOpinions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var webOpinion = WebOpinion.Get(id.Value);
            if (webOpinion == null)
            {
                return HttpNotFound();
            }
            return View(webOpinion);
        }

        // GET: Admin/WebOpinions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var webOpinion = WebOpinion.Get(id.Value);
            if (webOpinion == null)
            {
                return HttpNotFound();
            }
            return View(webOpinion);
        }

        // POST: Admin/WebOpinions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var webOpinion = await OpinionBussines.GetAsync(id);
            await webOpinion.RemoveAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
