using BusinessLayer;
using DomainModels.Interface;
using DomainModels.Models;
using System;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using DomainModels.Validators;

namespace WebUI.Controllers
{
    public class PollController : Controller
    {
        // GET: Poll
        public ActionResult MyPolls()
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            return View(ManagerFactory.PackageManager.GetMyPackage((IUserModel)Session["User"], Request.UserHostAddress).Select(x => new MyPoll { Id = x.Id, Name = x.Name, CountQuestions = x.Questions.Count }));
        }

        public ActionResult Edit(Guid id)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var edp = ManagerFactory.PackageManager.GetMyPackage((IUserModel)Session["User"], Request.UserHostAddress).FirstOrDefault(x => x.Id == id);
            return View(edp.CreateObject<EditPoll>());
        }
        [HttpPost]
        public ActionResult Edit(EditPoll model)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var pac = model.CreateObject<Package>();
            if (ValidatorFactory.ValidatorPackage.IsValidObject(pac))
                ManagerFactory.PackageManager.SavePackage(pac, (IUserModel)Session["User"], Request.UserHostAddress);
            return RedirectToAction("MyPolls");
        }
        public ActionResult Create()
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            return View("Edit", new EditPoll());
        }
        [HttpPost]
        public ActionResult Create(EditPoll model)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var pac = model.CreateObject<Package>();
            pac.Id = Guid.NewGuid();
            if (ValidatorFactory.ValidatorPackage.IsValidObject(pac))
                ManagerFactory.PackageManager.SavePackage(pac, (IUserModel)Session["User"], Request.UserHostAddress);
            return RedirectToAction("MyPolls");
        }
        public ActionResult Delete(Guid id)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var pac = new Package { Id = id };
            ManagerFactory.PackageManager.DeletePackage(pac, (IUserModel)Session["User"], Request.UserHostAddress);
            return RedirectToAction("MyPolls");
        }
    }
}