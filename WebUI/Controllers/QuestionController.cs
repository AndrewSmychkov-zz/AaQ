using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Models;
using BusinessLayer;
using DomainModels.Validators;
using Helpers;
using System.Web;
using WebUI.Models;
using System.Web.Mvc;
using DomainModels.Interface;

namespace WebUI.Controllers
{
    public class QuestionController : Controller
    {
        //
        // GET: /Question/
        public ActionResult Create(Guid id)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var qvw = new QuestionViewModel { PackageId = id };
            qvw.AnswerTypes = ManagerFactory.GlossaryManager.GetAllAnswerType((IUserModel)Session["User"], Request.UserHostAddress);
            return View("Edit", qvw);
        }
        public ActionResult MyQuestions(Guid id)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            IList<QuestionViewModel> list = new List<QuestionViewModel>();
            foreach (var q in ManagerFactory.QuestionManager.GetMyQuestions(new Package { Id = id }, (IUserModel)Session["User"], Request.UserHostAddress)) 
            {
                var qvw = q.CreateObject<QuestionViewModel>();
                qvw.AnswerTypeId = q.AnswerType.Id;
                qvw.PackageId = id;
                list.Add(qvw);
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Create(QuestionViewModel model)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            model.Id = Guid.NewGuid();
            var que = model.CreateObject<Question>();
            que.AnswerType = ManagerFactory.GlossaryManager.GetAllAnswerType((IUserModel)Session["User"], Request.UserHostAddress).FirstOrDefault(x => x.Id == model.AnswerTypeId);
            var strerror = ValidatorFactory.ValidatorQuestion.ValidatePropertys(que, new[] {"Id", "Text"});
            if (!strerror.Any())
            {
                var pac = new Package { Id = model.PackageId };
                ManagerFactory.QuestionManager.SaveQuestions(que, pac, (IUserModel)Session["User"], Request.UserHostAddress);
            }
            else 
            {
                foreach (var err in strerror) 
                {
                    if (model.GetType().GetProperty(err.Key) != null)
                    {
                        ModelState.AddModelError(err.Key, string.Concat(err.Value));
                    }
                    else 
                    {
                        ModelState.AddModelError("", string.Concat(err.Value));
                    }
                }
                model.AnswerTypes = ManagerFactory.GlossaryManager.GetAllAnswerType((IUserModel)Session["User"], Request.UserHostAddress);
                return View("Edit",model);
            }
            return Redirect("/Poll/MyPolls");
        }
           [HttpPost]
        public ActionResult Edit(QuestionViewModel model)
        {
            if (Session["User"] == null)
                return Redirect("/Account/LogOn");
            var qvw = new QuestionViewModel { PackageId = model.PackageId };
            qvw.AnswerTypes = ManagerFactory.GlossaryManager.GetAllAnswerType((IUserModel)Session["User"], Request.UserHostAddress);
            return View("Edit", qvw);
        }
    }
}