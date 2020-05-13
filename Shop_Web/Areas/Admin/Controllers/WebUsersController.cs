using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebUsersController : Controller
    {
        // GET: Admin/WebUsers
        public ActionResult Index()
        {
            return View(WebUsers.GetAll());
        }

        // GET: Admin/WebUsers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webUsers = WebUsers.Get(id.Value);
            if (webUsers == null)
            {
                return HttpNotFound();
            }
            return View(webUsers);
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new UserBussines
                    {
                        Guid = Guid.NewGuid(),
                        Modified = DateTime.Now,
                        ActiveCode = Guid.NewGuid().ToString(),
                        IsActive = true,
                        RegisterDate = DateTime.Now,
                        RememberMe = false,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(webUsers.Password, "MD5"),
                        Email = webUsers.Email,
                        RealName = webUsers.RealName,
                        UserName = webUsers.UserName
                    };
                    user.RolleGuid = WebUsers.GetRolleGuid(webUsers.IsAdmin ? "Admin" : "User");
                    user.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
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

            var webUsers = WebUsers.Get(id.Value);
            return View(webUsers);
        }

        // POST: Admin/WebUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,Modified,RealName,RolleGuid,UserName,Password,Email,ActiveCode,IsActive,RegisterDate,RememberMe,RePassword")] WebUsers webUsers)
        {
            if (string.IsNullOrEmpty(webUsers.RealName)) return View(webUsers);
            if (string.IsNullOrEmpty(webUsers.UserName)) return View(webUsers);
            if (string.IsNullOrEmpty(webUsers.Email)) return View(webUsers);
            var user = UserBussines.Get(webUsers.Guid);
            user.Email = webUsers.Email;
            user.RealName = webUsers.RealName;
            user.UserName = webUsers.UserName;
            user.RolleGuid = WebUsers.GetRolleGuid(webUsers.IsAdmin ? "Admin" : "User");
            user.Save();
            return RedirectToAction("Index");

        }

        // GET: Admin/WebUsers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webUsers = WebUsers.Get(id.Value);
            if (webUsers == null)
            {
                return HttpNotFound();
            }
            return View(webUsers);
        }

        // POST: Admin/WebUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var webUsers = WebUsers.Get(id);
            webUsers.Remove();
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
