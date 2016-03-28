using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Models;
using DomainModels.Validators;

namespace UnitTestDomainModels
{
    [TestClass]
    public class UnitTestValidatorUser
    {
        [TestMethod]
        public void ValidateUser()
        {
            var u = new User();
            u.Login = "логин";
            u.Id = Guid.NewGuid();
            u.Password = "пароль";
            Assert.IsTrue(ValidatorFactory.ValidatorUser.IsValidObject(u));
        }       
    }
}
