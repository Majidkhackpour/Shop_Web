using System;
using System.Net;
using System.Web.Mvc;
using EntityCache.WebBussines;

namespace Shop_Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WebUsersController : Controller
    {
        // GET: Admin/WebUsers
        public ActionResult Index()
        {
            //return View(db.Users.ToList());
            return View();
        }

        // GET: Admin/WebUsers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebUsers webUsers = db.WebUsers.Find(id);
            //if (webUsers == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webUsers);
            return View();
        }

        // GET: Admin/WebUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/WebUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Guid,Modified,RealName,RolleGuid,UserName,Password,Email,ActiveCode,IsActive,RegisterDate,RememberMe,RePassword")] WebUsers webUsers)
        {
            if (ModelState.IsValid)
            {
                //webUsers.Guid = Guid.NewGuid();
                //db.WebUsers.Add(webUsers);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(webUsers);
        }

        // GET: Admin/WebUsers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //}
            //WebUsers webUsers = db.WebUsers.Find(id);
            //if (webUsers == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webUsers);
            return View();
        }

        // POST: Admin/WebUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,Modified,RealName,RolleGuid,UserName,Password,Email,ActiveCode,IsActive,RegisterDate,RememberMe,RePassword")] WebUsers webUsers)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(webUsers).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(webUsers);
        }

        // GET: Admin/WebUsers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebUsers webUsers = db.WebUsers.Find(id);
            //if (webUsers == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webUsers);
            return View();
        }

        // POST: Admin/WebUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            //WebUsers webUsers = db.WebUsers.Find(id);
            //db.WebUsers.Remove(webUsers);
            //db.SaveChanges();
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
