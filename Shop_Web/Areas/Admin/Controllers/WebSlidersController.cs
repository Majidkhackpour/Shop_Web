using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using Shop_Web.Utilities;

namespace Shop_Web.Areas.Admin.Controllers
{
    public class WebSlidersController : Controller
    {

        // GET: Admin/WebSliders
        public ActionResult Index()
        {
            return View(WebSlider.GetAll());
        }

        // GET: Admin/WebSliders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebSlider webSlider = db.WebSliders.Find(id);
            //if (webSlider == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webSlider);
            return View();
        }

        // GET: Admin/WebSliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/WebSliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Guid,Modified,Title,ImageName,URL,StartDate,EndDate,IsActive")] WebSlider webSlider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp == null || !imgUp.IsImage())
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر را انتخاب نمایید");
                    return View(webSlider);
                }

                var s = new SliderBussines()
                {
                    Guid = Guid.NewGuid(),
                    IsActive = webSlider.IsActive,
                    Modified = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    Title = webSlider.Title,
                    ImageName = Guid.NewGuid() + Path.GetExtension(imgUp.FileName),
                    URL = webSlider.URL
                };

                imgUp.SaveAs(Server.MapPath("/Images/Slider/" + s.ImageName));

                await s.SaveAsync();

                return RedirectToAction("Index");
            }

            return View(webSlider);
        }

        // GET: Admin/WebSliders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webSlider = WebSlider.Get(id.Value);
            if (webSlider == null)
            {
                return HttpNotFound();
            }
            return View(webSlider);
        }

        // POST: Admin/WebSliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Guid,Modified,Title,ImageName,URL,StartDate,EndDate,IsActive")] WebSlider webSlider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                var s = SliderBussines.Get(webSlider.Guid);
                if (imgUp != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Images/Slider/" + webSlider.ImageName));
                    s.ImageName = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Slider/" + s.ImageName));
                }

                s.IsActive = webSlider.IsActive;
                s.Title = webSlider.Title;
                s.URL = webSlider.URL;

                await s.SaveAsync();

                return RedirectToAction("Index");
            }
            return View(webSlider);
        }

        // GET: Admin/WebSliders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var webSlider = WebSlider.Get(id.Value);
            if (webSlider == null)
            {
                return HttpNotFound();
            }
            return View(webSlider);
        }

        // POST: Admin/WebSliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var s = await SliderBussines.GetAsync(id);
            System.IO.File.Delete(Server.MapPath("/Images/Slider/" + s.ImageName));
            await s.RemoveAsync();
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
