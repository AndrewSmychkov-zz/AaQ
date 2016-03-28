using DomainModels.Interface;
using System;
using System.Collections.Generic;


namespace DomainModels.Validators
{
    internal sealed class ValidatorAnswer : ValidatorBase<IAnswerModel>
    {
        public ValidatorAnswer()
        {           
            dic.Add("Name", new[] { "Ответ должен быть в диапазоне от 1 до 250 символов." });
            dic.Add("Id", new[] { "Пустой идентификатор ответ" });
        }
        protected override void CheckProperty(object value, IList<string> errors, string propertyName)
        {
            switch (propertyName)
            {
                case "Name":
                    if (!ValidateString((string)value, @"^.{1,250}$", true))
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
