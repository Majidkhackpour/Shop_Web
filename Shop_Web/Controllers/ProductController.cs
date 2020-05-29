using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.ViewModels;
using EntityCache.WebBussines;
using PacketParser.Services;
using Shop_Web.MoreLinq;

namespace Shop_Web.Controllers
{
    public class ProductController : Controller
    {
        public async Task<ActionResult> ShowGroups()
        {
            var list = await WebProductGroup.GetAllAsync();
            return PartialView(list);
        }

        public ActionResult LastView()
        {
            var list = WebProduct.GetAll().OrderByDescending(q => q.CreateDate).Take(12).ToList();
            return PartialView(list);
        }
        [Route("ShowProduct/{id}")]
        public ActionResult ShowProduct(Guid id)
        {
            var prd = WebProduct.Get(id);
            if (prd == null) return HttpNotFound();
            ViewBag.PrdFeatures = prd.FeatureList.DistinctBy(q => q.FeatureGuid).Select(q => new ShowPrdFeaturesVireModel()
            {
                FeatureTitle = q.FeatureName,
                Values = prd.FeatureList.Where(h => h.FeatureGuid == q.FeatureGuid).Select(h => h.Value).ToList()
            }).ToList();
            return View(prd);
        }

        public ActionResult ShowComment(Guid id)
        {
            var list = WebPrdComment.GetAll(id);
            return PartialView(list);
        }

        public ActionResult CreateComment(Guid id)
        {
            return PartialView(new WebPrdComment()
            {
                Guid = Guid.NewGuid(),
                PrdGuid = id
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment(WebPrdComment webPrd)
        {
            try
            {
                if (!ModelState.IsValid) return PartialView(webPrd);
                var prd = new PrdCommentBussines()
                {
                    Guid = webPrd.Guid,
                    ParentGuid = webPrd.ParentGuid,
                    Modified = DateTime.Now,
                    Name = webPrd.Name,
                    Email = webPrd.Email,
                    PrdGuid = webPrd.PrdGuid,
                    Comment = webPrd.Comment,
                    WebSite = webPrd.WebSite,
                    CreateDate = DateTime.Now
                };
                if (prd.Guid == Guid.Empty)
                    prd.Guid = Guid.NewGuid();
                await prd.SaveAsync();
                return PartialView("ShowComment", WebPrdComment.GetAll(prd.PrdGuid));
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return PartialView(webPrd);
        }

        [Route("Archive")]
        public ActionResult ArchiveProduct(int pageId = 1, string title = "", int minPrice = 0, int maxPrice = 0, List<Guid> selectedGroups = null)
        {
            var list = WebProductGroup.GetAll();
            ViewBag.Groups = list;
            ViewBag.PrdTitle = title;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.pageId = pageId;
            ViewBag.selectGroup = selectedGroups;

            var prdList = WebProduct.GetAll(title, minPrice, maxPrice, selectedGroups);

            var take = 9;
            var skip = (pageId - 1) * take;
            ViewBag.PageCount = prdList.Count() / take;
            return View(prdList.OrderByDescending(q => q.CreateDate).Skip(skip).Take(take).ToList());
        }
    }
}