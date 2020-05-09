using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using EntityCache.Bussines;
using EntityCache.WebBussines;

namespace Shop_Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult RightSide()
        {
            return PartialView("RightSide");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Register(WebUsers user)
        {
            if (!ModelState.IsValid) return View(user);
            if(!await WebUsers.CheckEmail(user.Guid,user.Email))
                ModelState.AddModelError("Email","ایمیل وارد شده از پیش در سایت ثبت نام کرده است");
            var webuser = new UserBussines()
            {
                Guid = Guid.NewGuid(),
                Email = user.Email.Trim().ToLower(),
                Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "MD5"),
                ActiveCode = Guid.NewGuid().ToString(),
                IsActive = false,
                Modified = DateTime.Now,
                RealName = user.RealName,
                RegisterDate = DateTime.Now,
                UserName = user.UserName,
                RolleGuid = WebUsers.GetRolleGuid("User")
            };
            await webuser.SaveAsync();
            return View("_SuccesRegister", webuser);
        }
    }
}