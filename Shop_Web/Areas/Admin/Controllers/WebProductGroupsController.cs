using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityCache.Assistence;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebProductGroupsController : Controller
    {
        // GET: Admin/WebProductGroups
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LiatGroups()
        {
            return PartialView(WebProductGroup.GetAll().Where(q => q.ParentGuid == Guid.Empty).OrderBy(q => q.Code));
        }

        // GET: Admin/WebProductGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webProductGroup = WebProductGroup.Get(id.Value);
            if (webProductGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView("LiatGroups",
                WebProductGroup.GetAll().Where(q => q.ParentGuid == Guid.Empty).OrderBy(q => q.Code));
        }

        // GET: Admin/WebProductGroups/Create
        public ActionResult Create(Guid? id)
        {
            return PartialView(new WebProductGroup()
            {
                ParentGuid = id ?? Guid.Empty
            });
        }

        // POST: Admin/WebProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Guid,Modified,Code,Name,ParentGuid,Description")] WebProductGroup webProductGroup)
        {
            try
            {
                if (string.IsNullOrEmpty(webProductGroup.Name)) return View(webProductGroup);

                var prdGroup = new ProductGroupBussines
                {
                    Guid = Guid.NewGuid(),
                    ParentGuid = webProductGroup.ParentGuid,
                    Name = webProductGroup.Name,
                    Modified = DateTime.Now,
                    Code = await ProductGroupBussines.NextCode(),
                    Description = ""
                };

                await prdGroup.SaveAsync();
                return PartialView("LiatGroups",
                    WebProductGroup.GetAll().Where(q => q.ParentGuid == Guid.Empty).OrderBy(q => q.Code));
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return View(webProductGroup);
        }

        // GET: Admin/WebProductGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webProductGroup = WebProductGroup.Get(id.Value);
            if (webProductGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(webProductGroup);
        }

        // POST: Admin/WebProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Guid,Modified,Code,Name,ParentGuid,Description")] WebProductGroup webProductGroup)
        {
            try
            {
                if (string.IsNullOrEmpty(webProductGroup.Name)) return View(webProductGroup);
                var grp = await ProductGroupBussines.GetAsync(webProductGroup.Guid);
                if (grp == null) return View(webProductGroup);
                grp.Name = webProductGroup.Name;
                await grp.SaveAsync();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
            return PartialView("LiatGroups",
                WebProductGroup.GetAll().Where(q => q.ParentGuid == Guid.Empty).OrderBy(q => q.Code));
        }

        // GET: Admin/WebProductGroups/Delete/5
        public async Task Delete(Guid id)
        {
            try
            {
                var grp = await ProductGroupBussines.GetAsync(id);
                if (grp == null) return;
                if (await ProductGroupBussines.ChildCount(id) > 0)
                {
                    var childs = await ProductGroupBussines.GetAllAsync();
                    childs = childs.Where(q => q.ParentGuid == id).ToList();
                    foreach (var item in childs)
                    {
                        await item.RemoveAsync();
                    }
                }
                await grp.RemoveAsync();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
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
                //UnitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
