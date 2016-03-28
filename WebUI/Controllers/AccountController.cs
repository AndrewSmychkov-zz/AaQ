using DomainModels.Models;
using DomainModels.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;
using System.Web.Mvc;
using WebUI.Models;
using BusinessLayer;
using DomainModels.Interface;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            if (Session["User"] != null && Session["User"] is IUserModel)
                return Redirect("/Poll/MyPolls");
            else
                return View();
        }
        [HttpPost]
        public ActionResult LogOn(LogOnUser model)
        {
            if (ModelState.IsValid)
            {
                var user = model.CreateObject<User>();
                var validstr = ValidatorFactory.ValidatorUser.ValidatePropertys(user, new[] { "Login", "Password" });
                if (validstr.Any())
                {
                    foreach (var fielderror in validstr)
                    {
                        ModelState.AddModelError(fielderror.Key, string.Concat(fielderror.Value));
                    }
                    return View(model);
                }
                Session["User"] = ManagerFactory.UserManager.LogOn(user, Request.UserHostAddress);
                return  Redirect("/Poll/MyPolls");
            }

            return View(model);
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();           
            return RedirectToAction("LogOn");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var user = model.CreateObject<User>();
                user.Id = Guid.NewGuid();
                var validstr = ValidatorFactory.ValidatorUser.ValidateObject(user);
                if (validstr.Any())
                {
                    foreach (var fielderror in validstr)
                    {
                        ModelState.AddModelError(fielderror.Key, string.Concat(fielderror.Value));
                    }
                    return View(model);
                }
                if (ManagerFactory.UserManager.RegisterUser(user))
                 return  RedirectToAction("LogOn");
                else
                    ModelState.AddModelError("", "Ошибка регитсрации");
            }

            return View(model);
        }
    }
}