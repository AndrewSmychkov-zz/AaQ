using DomainModels.Interface;
using System;
using System.Linq;
using System.Collections.Generic;


namespace DomainModels.Validators
{
    internal sealed class ValidatorPackage : ValidatorBase<IPackageModel>
    {
        public ValidatorPackage()
        {
            dic.Add("Name", new[] { "Название опросника должно быть в диапазоне от 1 до 250 символов." });
            dic.Add("Id", new[] { "Пустой идентификатор ответ" });
            dic.Add("Questions", new[] { "Опросник должен содержать вопросы", "Ошибка в вопросе: ", "В опроснике не должны быть одинаковые вопросы" });
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
                case "Questions":
                    if (value != null)
                    {
                        var que = (IEnumerable<IQuestionModel>)value;
                        if (que.Count() != que.Select(x => x.Text.ToLower()).Distinct().Count())
                        {
                            errors.Add(GetErrorStringFor("Questions")[2]);
                            break;
                        }
                        if (que.Any())
                        {
                            foreach (IQuestionModel model in que.Where(x => !ValidatorFactory.ValidatorQuestion.IsValidObject(x)))
                            {
                                errors.Add(GetErrorStringFor("Questions")[1] + model.Text);
                            }
                        }
                        else
                        {
                            errors.Add(GetErrorStringFor("Questions")[0]);
                        }
                    }
                    break;

            }
        }
    }
}
