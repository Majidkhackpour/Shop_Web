﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using EntityCache.Bussines;
using EntityCache.WebBussines;
using PacketParser.Services;
using Shop_Web.Utilities;

namespace Shop_Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Register(WebUsers user, FormCollection form)
        {
            try
            {
                if (!ModelState.IsValid) return View(user);
                if (!GoogleRecaptcha.Authentication(form)) return View(user);
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
                    RolleGuid = WebUsers.GetRolleGuid("User"),
                    RememberMe = false
                };

                var body =
                    PartialToStringClass.RenderPartialView("_ManageEmails", "ActivationEmail", webuser);

                SendEmail.Send(webuser.Email, "ایمیل فعالسازی", body);

                await webuser.SaveAsync();

                return View("_SuccesRegister", webuser);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return View(user);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(WebUsers login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Email))
                {
                    ModelState.AddModelError("Email", "لطفا ایمیل را وارد نمایید");
                    return View(login);
                }
                if (string.IsNullOrEmpty(login.Password))
                {
                    ModelState.AddModelError("Password", "لطفا رمز عبور را وارد نمایید");
                    return View(login);
                }
                var hashPass = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = await UserBussines.AuthenticationUserAsync(login.Email, hashPass);
                if (user != null)
                {
                    //کاربر پیدا شد
                    if (user.IsActive)
                    {
                        user.RememberMe = login.RememberMe;
                        await user.SaveAsync();
                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                        return Redirect("/");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد");
                }
                return View(login);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return View(login);
            }
        }
        public async Task<ActionResult> ActiveUser(string id)
        {
            try
            {
                var user = await UserBussines.GetAsync(id);
                if (user == null) return HttpNotFound();
                user.IsActive = true;
                user.ActiveCode = Guid.NewGuid().ToString();
                await user.SaveAsync();
                ViewBag.RealName = user.RealName;
                return View();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return View();
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}