using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.ViewModels;
using EntityCache.WebBussines;

namespace Shop_Web.Controllers
{
    public class ShopCartController : Controller
    {
        // GET: ShopCart
        public ActionResult ShowCart()
        {
            var list = new List<OrderItemViewModel>();

            if (Session["ShopCart"] != null)
            {
                var listShop = (List<OrderItem>)Session["ShopCart"];
                
                foreach (var item in listShop)
                {
                    var prd = WebProduct.Get(item.PrdGuid);
                    list.Add(new OrderItemViewModel()
                    {
                        PrdGuid = item.PrdGuid,
                        Count = item.Count,
                        ImageName = prd.ImageName,
                        Title = prd.Name
                    });
                }
            }

            return PartialView(list);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            return PartialView(GetOrderList());
        }

        public ActionResult OrderCommand(Guid id, int count)
        {
            var listShop = Session["ShopCart"] as List<OrderItem>;
            var index = listShop.FindIndex(q => q.PrdGuid == id);
            if (count == 0)  listShop.RemoveAt(index);
            else listShop[index].Count = count;

            Session["ShopCart"] = listShop;

            return PartialView("Order", GetOrderList());
        }

        List<ShowOrderViewModel> GetOrderList()
        {
            var list = new List<ShowOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                var listShop = Session["ShopCart"] as List<OrderItem>;
                foreach (var item in listShop)
                {
                    var prd = WebProduct.Get(item.PrdGuid);
                    list.Add(new ShowOrderViewModel()
                    {
                        PrdGuid = item.PrdGuid,
                        Count = item.Count,
                        ImageName = prd.ImageName,
                        Title = prd.Name,
                        Price = prd.Price,
                        Sum = item.Count * prd.Price
                    });
                }
            }

            return list;
        }

        [Authorize]
        public async Task<ActionResult> PayMent()
        {
            var user = await UserBussines.GetAsyncByUserName(User.Identity.Name);
            var order = new OrderBussines()
            {
                Guid = Guid.NewGuid(),
                CustomerGuid = user.Guid,
                Date = DateTime.Now,
                Modified = DateTime.Now,
                IsFinally = false,
                OrderNo = await OrderBussines.GetMaxOrderNo()
            };

            var detList = GetOrderList();
            var detailList=new List<OrderDetailBussines>();
            foreach (var item in detList)
            {
                var det = new OrderDetailBussines()
                {
                    PrdGuid = item.PrdGuid,
                    Count = item.Count,
                    Price = item.Price,
                    Guid = Guid.NewGuid(),
                    Modified = DateTime.Now,
                    OrderGuid = order.Guid
                };
                detailList.Add(det);
            }

            order.DetList = detailList;
            await order.SaveAsync();

            //TODO : OnlinePayment
            // بعد از برگشت ار درگاه باید isfinally=true شود

            return null;
        }

    }
}