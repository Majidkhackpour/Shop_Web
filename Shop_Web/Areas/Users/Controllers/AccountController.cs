using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;

namespace Shop_Web.Areas.Users.Controllers
{
    public class AccountController : Controller
    {
        // GET: Users/Account
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(WebUsers change)
        {
            try
            {
                if (string.IsNullOrEmpty(change.UserName))
                {
                    ModelState.AddModelError("UserName", "لطفا نام کاربری را وارد نمایید");
                    return View(change);
                }
                if (string.IsNullOrEmpty(change.Password))
                {
                    ModelState.AddModelError("Password", "لطفا رمز عبور را وارد نمایید");
                    return View(change);
                }
                if (string.IsNullOrEmpty(change.RePassword))
                {
                    ModelState.AddModelError("RePassword", "لطفا تکرار رمز عبور را وارد نمایید");
                    return View(change);
                }

                var user = await UserBussines.GetAsyncByUserName(change.UserName);
                var hashNewPass = FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
                user.Password = hashNewPass;
                await user.SaveAsync();
                ViewBag.Success = true;

                return View();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return View();
            }
        }
    }
}