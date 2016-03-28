using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Models;
using DomainModels.Validators;

namespace UnitTestDomainModels
{
    [TestClass]
    public class UnitTestValidatorQuestions
    {
        [TestMethod]
        public void ValidateQuestion()
        {
            var u = new Question();
            u.AnswerType = new GlossaryItem(3, "", "");//фейковый тип вопроса
            u.Id = Guid.NewGuid();
            u.Text = "текст";
            Assert.IsTrue(ValidatorFactory.ValidatorQuestion.IsValidObject(u));
        }
    }
}
