using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;
using SqlServerPersistence.Model;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebFeaturesController : Controller
    {

        // GET: Admin/WebFeatures
        public ActionResult Index()
        {
            return View(WebFeature.GetAll());
        }

        // GET: Admin/WebFeatures/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webFeature = WebFeature.Get(id.Value);
            if (webFeature == null)
            {
                return HttpNotFound();
            }
            return View(webFeature);
        }

        // GET: Admin/WebFeatures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/WebFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Guid,Modified,Title")] WebFeature webFeature)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = new FeatureBussines()
                    {
                        Guid = Guid.NewGuid(),
                        Modified = DateTime.Now,
                        Title = webFeature.Title
                    };
                    await f.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/WebFeatures/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var webFeature = WebFeature.Get(id.Value);
            if (webFeature == null)
            {
                return HttpNotFound();
            }
            return View(webFeature);
        }

        // POST: Admin/WebFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Guid,Modified,Title")] WebFeature webFeature)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = FeatureBussines.Get(webFeature.Guid);
                    if (f == null) return View(webFeature);
                    f.Title = webFeature.Title;
                    await f.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/WebFeatures/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var webFeature = WebFeature.Get(id.Value);
            if (webFeature == null)
            {
                return HttpNotFound();
            }
            return View(webFeature);
        }

        // POST: Admin/WebFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var f = FeatureBussines.Get(id);
                if (f == null) return RedirectToAction("Index");
                await f.RemoveAsync();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

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
