using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Register(WebUsers user)
        {
            try
            {
                if (!ModelState.IsValid) return View(user);
                if (user.Guid == Guid.Empty)
                    user.Guid = Guid.NewGuid();
                if (!await WebUsers.CheckEmail(user.Guid, user.Email))
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده از پیش در سایت ثبت نام کرده است");
                    return View(user);
                }

                var webuser = new UserBussines()
                {
                    Guid = user.Guid,
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
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return View(user);
            }
        }
    }
}