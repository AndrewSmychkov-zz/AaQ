using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Models;
using DomainModels.Validators;

namespace UnitTestDomainModels
{
    [TestClass]
    public class UnitTestValidatorAnswer
    {
        [TestMethod]
        public void ValidateAnswer()
        {
            var a = new Answer();
            a.Name = "имя";
            a.Id = Guid.NewGuid();          
            Assert.IsTrue(ValidatorFactory.ValidatorAnswer.IsValidObject(a));
        }
    }
}
