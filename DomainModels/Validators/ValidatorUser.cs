using DomainModels.Interface;
using System;
using System.Collections.Generic;

namespace DomainModels.Validators
{
    internal sealed class ValidatorUser : ValidatorBase<IUserModel>
    {
        public ValidatorUser()
        {
            dic.Add("Password", new[] { "Пароль должен быть не меньше 2 символов.","Очееень большой пароль" });
            dic.Add("Login", new[] { "Логин должен быть в диапазоне от 1 до 50 символов." });         
            dic.Add("Id", new[] { "Пустой идентификатор пользователя" });
        }
        protected override void CheckProperty(object value, IList<string> errors, string propertyName)
        {
            switch (propertyName)
            {
                case "Password":                    
                    if (!ValidateString((string)value, @"^.{0,8000}$", true))
                        errors.Add(GetErrorStringFor(propertyName)[1]);
                    if (!ValidateString((string)value, @"^.{2,}$", true))
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;              
                case "Login":
                    if (!ValidateString((string)value, @"^.{1,50}$", true))
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;
                case "Id":
                    if ((Guid)value == Guid.Empty)
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;

            }
        }        
    }
}
