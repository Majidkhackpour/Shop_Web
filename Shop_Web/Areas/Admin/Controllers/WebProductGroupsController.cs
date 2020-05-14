using System;
using System.Linq;
using System.Web.Mvc;
using EntityCache.WebBussines;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebProductGroupsController : Controller
    {
        // GET: Admin/WebProductGroups
        public ActionResult Index()
        {
            return View(WebProductGroup.GetAll().Where(q=>q.ParentGuid==Guid.Empty).OrderBy(q=>q.Code));
        }

        // GET: Admin/WebProductGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //WebProductGroup webProductGroup = db.WebProductGroups.Find(id);
            //if (webProductGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webProductGroup);
            return View();
        }

        // GET: Admin/WebProductGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/WebProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Guid,Modified,Code,Name,ParentGuid,Description")] WebProductGroup webProductGroup)
        {
            //if (ModelState.IsValid)
            //{
            //    webProductGroup.Guid = Guid.NewGuid();
            //    db.WebProductGroups.Add(webProductGroup);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(webProductGroup);
            return View();
        }

        // GET: Admin/WebProductGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //WebProductGroup webProductGroup = db.WebProductGroups.Find(id);
            //if (webProductGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webProductGroup);
            return View();
        }

        // POST: Admin/WebProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,Modified,Code,Name,ParentGuid,Description")] WebProductGroup webProductGroup)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(webProductGroup).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(webProductGroup);
            return View();
        }

        // GET: Admin/WebProductGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //WebProductGroup webProductGroup = db.WebProductGroups.Find(id);
            //if (webProductGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webProductGroup);
            return View();
        }

        // POST: Admin/WebProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            //WebProductGroup webProductGroup = db.WebProductGroups.Find(id);
            //db.WebProductGroups.Remove(webProductGroup);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return View();
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
