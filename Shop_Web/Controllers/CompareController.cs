using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.ViewModels;
using EntityCache.WebBussines;
using PacketParser.Services;
using Shop_Web.MoreLinq;

namespace Shop_Web.Controllers
{
    public class CompareController : Controller
    {
        // GET: Compare
        public ActionResult Index()
        {
            var list = new List<CompareItem>();

            if (Session["Compare"] != null)
                list = Session["Compare"] as List<CompareItem>;

            var features = new List<WebFeature>();
            var prdFetaures=new List<PrdFeatureBussines>();
            foreach (var item in list)
            {
                var prdfeature = WebProduct.Get(item.PrdGuid).FeatureList;
                prdFetaures.AddRange(prdfeature);
                foreach (var bussinese in prdfeature)
                {
                    var f = WebFeature.Get(bussinese.FeatureGuid);
                    features.Add(f);
                }
            }

            var a = features.DistinctBy(q => q.Title).ToList();
            ViewBag.Features = a;
            ViewBag.prdFetaures = prdFetaures;
            return View(list);
        }

        public ActionResult AddToCompare(Guid id)
        {
            var list = new List<CompareItem>();
            try
            {
                if (Session["Compare"] != null)
                    list = Session["Compare"] as List<CompareItem>;

                if (!list.Any(q => q.PrdGuid == id))
                {
                    var prd = WebProduct.Get(id);
                    list.Add(new CompareItem()
                    {
                        PrdGuid = id,
                        ImageName = prd.ImageName,
                        PrdTitle = prd.Name
                    });
                }

                Session["Compare"] = list;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return PartialView("ListCompare", list);
        }

        public ActionResult ListCompare()
        {
            var list = new List<CompareItem>();

            if (Session["Compare"] != null)
                list = Session["Compare"] as List<CompareItem>;

            return PartialView(list);
        }

        public ActionResult DeleteCompare(Guid id)
        {
            var list = new List<CompareItem>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItem>;
                var index = list.FindIndex(q => q.PrdGuid == id);
                list.RemoveAt(index);
            }

            return PartialView("ListCompare", list);
        }
    }
}