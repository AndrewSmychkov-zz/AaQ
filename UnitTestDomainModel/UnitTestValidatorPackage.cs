using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Models;
using DomainModels.Validators;

namespace UnitTestDomainModels
{
    [TestClass]
    public class UnitTestValidatorPackage
    {
        [TestMethod]
        public void ValidatePackage()
        {
            var q1 = new Question();
            q1.AnswerType = new GlossaryItem(1, "", "");//фейковый тип вопроса
            q1.Id = Guid.NewGuid();
            q1.Text = "текст";
            q1.Answers.Add(new Answer { Id = Guid.NewGuid(), Name = "первый" });
            q1.Answers.Add(new Answer { Id = Guid.NewGuid(), Name = "второй" });
            var q2 = new Question();
            q2.AnswerType = new GlossaryItem(1, "", "");//фейковый тип вопроса
            q2.Id = Guid.NewGuid();
            q2.Text = "текст2";
            q2.Answers.Add(new Answer { Id = Guid.NewGuid(), Name = "первый" });
            q2.Answers.Add(new Answer { Id = Guid.NewGuid(), Name = "второй" });         

            var p = new Package { Id = Guid.NewGuid(), Name = "опросник" };
            p.Questions.Add(q1);
            p.Questions.Add(q2);
            Assert.IsTrue(ValidatorFactory.ValidatorPackage.IsValidObject(p));
        }
    }
}
