using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EntityCache.ViewModels;

namespace Shop_Web.Controllers
{
    public class OrderController : ApiController
    {
        // GET: api/Order
        public int Get()
        {
            var list = new List<OrderItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["ShopCart"] != null)
            {
                //خرید nام
                list = (List<OrderItem>)sessions["ShopCart"];
            }

            return list.Sum(q => q.Count);
        }

        // GET: api/Order/5
        public int Get(Guid id)
        {
            var list = new List<OrderItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["ShopCart"] != null)
            {
                //خرید nام
                list = (List<OrderItem>)sessions["ShopCart"];
            }

            if (list.Any(q => q.PrdGuid == id))
            {
                //خرید کالای تکراری
                var index = list.FindIndex(q => q.PrdGuid == id);
                list[index].Count += 1;
            }
            else
            {
                //کالای غیر تکراری
                list.Add(new OrderItem()
                {
                    PrdGuid = id,
                    Count = 1
                });
            }

            sessions["ShopCart"] = list;

            return Get();
        }
    }
}
