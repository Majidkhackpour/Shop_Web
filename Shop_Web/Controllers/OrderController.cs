using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Shop_Web.Controllers
{
    public class OrderController : ApiController
    {
        // GET: api/Order
        public int Get()
        {
            var count = 0;

            return count;
        }

        // GET: api/Order/5
        public string Get(Guid id)
        {
            return "value";
        }
    }
}
