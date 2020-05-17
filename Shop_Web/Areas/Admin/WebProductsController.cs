using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;
using Shop_Web.Utilities;

namespace Shop_Web.Areas.Admin
{
    public class WebProductsController : Controller
    {

        // GET: Admin/WebProducts
        public ActionResult Index()
        {
            return View(WebProduct.GetAll().OrderBy(q => q.Name).ToList());
        }

        // GET: Admin/WebProducts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebProduct webProduct = db.WebProducts.Find(id);
            //if (webProduct == null)
            //{
            //    return HttpNotFound();
            //}
            // return View(webProduct);
            return View();
        }

        // GET: Admin/WebProducts/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var list = await ProductGroupBussines.GetAllAsync();
                ViewBag.Groups = list.ToList();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return View();
        }

        // POST: Admin/WebProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Guid,Modified,Code,Name,CreateDate,ImageName,Price,Description,ShortDesc,Abad,Kind,Color")]
            WebProduct webProduct, List<Guid> selectedGroups, HttpPostedFileBase imageproduct, string tags)
        {
            try
            {
                if (!ModelState.IsValid) return View(webProduct);
                if (selectedGroups == null || selectedGroups.Count <= 0)
                {
                    ViewBag.ErrorSelectedGroup = true;
                    var list2 = await ProductGroupBussines.GetAllAsync();
                    ViewBag.Groups = list2.ToList();
                    return View(webProduct);
                }

                var prd = new ProductBussines
                {
                    Guid = Guid.NewGuid(),
                    Name = webProduct.Name,
                    Modified = DateTime.Now,
                    Code = await ProductBussines.NextCode(),
                    Description = webProduct.Description.Replace("<p>", "").Replace("</p>", ""),
                    Abad = webProduct.Abad,
                    Color = webProduct.Color,
                    CreateDate = DateTime.Now,
                    Kind = webProduct.Kind,
                    Price = webProduct.Price,
                    ShortDesc = webProduct.ShortDesc,
                    ImageName = "No.png"
                };
                if (imageproduct != null && imageproduct.IsImage())
                {
                    prd.ImageName = Guid.NewGuid() + Path.GetExtension(imageproduct.FileName);
                    imageproduct.SaveAs(Server.MapPath("/Images/ProductImages/" + prd.ImageName));
                    var img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + prd.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + prd.ImageName));
                }

                var list = new List<PrdSelectedGroupBussines>();
                foreach (var item in selectedGroups)
                {
                    var a = new PrdSelectedGroupBussines()
                    {
                        Guid = Guid.NewGuid(),
                        Modified = DateTime.Now,
                        PrdGuid = prd.Guid,
                        GroupGuid = item
                    };
                    list.Add(a);
                }

                prd.GroupList = list;


                if (!string.IsNullOrEmpty(tags))
                {
                    var tag = tags.Split(',');
                    var tagList = new List<PrdTagBussines>();
                    foreach (var t in tag)
                    {
                        var a = new PrdTagBussines()
                        {
                            Guid = Guid.NewGuid(),
                            Modified = DateTime.Now,
                            PrdGuid = prd.Guid,
                            Tag = t.Trim()
                        };
                        tagList.Add(a);
                    }

                    prd.TagsList = tagList;
                }

                await prd.SaveAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            var list3 = await ProductGroupBussines.GetAllAsync();
            ViewBag.Groups = list3.ToList();
            return View();
        }

        // GET: Admin/WebProducts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebProduct webProduct = db.WebProducts.Find(id);
            //if (webProduct == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webProduct);
            return View();
        }

        // POST: Admin/WebProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,Modified,Code,Name,CreateDate,ImageName,Price,Description,ShortDesc,Abad,Kind,Color")] WebProduct webProduct)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(webProduct).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(webProduct);
            return View();
        }

        // GET: Admin/WebProducts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //WebProduct webProduct = db.WebProducts.Find(id);
            //if (webProduct == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(webProduct);
            return View();
        }

        // POST: Admin/WebProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            //WebProduct webProduct = db.WebProducts.Find(id);
            //db.WebProducts.Remove(webProduct);
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
