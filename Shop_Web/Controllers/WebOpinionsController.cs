using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using SqlServerPersistence.Model;

namespace Shop_Web.Controllers
{
    public class WebOpinionsController : Controller
    {
        // GET: WebOpinions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WebOpinions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Guid,Modified,Name,Email,Opinion")] WebOpinion webOpinion)
        {
            if (!ModelState.IsValid) return View(webOpinion);
            var op = new OpinionBussines()
            {
                Guid = Guid.NewGuid(),
                Opinion = webOpinion.Opinion,
                Email = webOpinion.Email,
                Name = webOpinion.Name,
                Modified = DateTime.Now
            };

            await op.SaveAsync();

            return Redirect("/");
        }
    }
}
