using System;
using System.Linq;
using System.Web.Mvc;
using EntityCache.ViewModels;
using EntityCache.WebBussines;
using PacketParser.Services;
using Shop_Web.MoreLinq;

namespace Shop_Web.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult ShowGroups()
        {
            return PartialView(WebProductGroup.GetAll());
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
                Values = prd.FeatureList.Where(h=>h.FeatureGuid==q.FeatureGuid).Select(h => h.Value).ToList()
            }).ToList();
            return View(prd);
        }
    }
}