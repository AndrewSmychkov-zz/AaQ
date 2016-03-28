using DomainModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DomainModels.Validators
{
    internal sealed class ValidatorQuestion : ValidatorBase<IQuestionModel>
    {
        public ValidatorQuestion()
        {
            dic.Add("Text", new[] { "Название опросника должно быть в диапазоне от 1 до 250 символов." });
            dic.Add("Id", new[] { "Пустой идентификатор ответ" });
            dic.Add("AnswerType", new[] { "Необходимо указать тип вопроса" });
            dic.Add("Answers", new[] { "Вопрос должен содержать ответы", "Ошибка в ответе: ", "В вопросе не должны быть одинаковые ответы", "Вопрос не должен содержать ответы" });
        }
        protected override void CheckProperty(object value, IList<string> errors, string propertyName)
        {
            switch (propertyName)
            {
                case "Text":
                    if (!ValidateString((string)value, @"^.{1,250}$", true))
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;
                case "Id":
                    if ((Guid)value == Guid.Empty)
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;
                case "AnswerType":
                    if (value == null)
                        errors.Add(GetErrorStringFor(propertyName)[0]);
                    break;
                case "Answers":
                    if (value != null)
                    {
                        var an = (IEnumerable<IAnswerModel>)value;
                        if (an.Count() != an.Select(x => x.Name.ToLower()).Distinct().Count())
                        {
                            errors.Add(GetErrorStringFor("Answers")[2]);
                            break;
                        }
                        if (an.Any())
                        {
                            foreach (IAnswerModel model in an.Where(x => !ValidatorFactory.ValidatorAnswer.IsValidObject(x)))
                            {
                                errors.Add(GetErrorStringFor("Answers")[1] + model.Name);
                            }
                        }                       
                    }
                    break;

            }
        }
        protected override void CheckProperty(IQuestionModel context, IList<string> errors, PropertyInfo val)
        {             
            if (val == null) return;
            switch (val.Name)
            {
                case "Answers":
                    var ans = (IEnumerable<IAnswerModel>)val.GetValue(context);
                    var at = (IGlossaryItemModel)context.GetType().GetRuntimeProperty("AnswerType").GetValue(context);
                    if (at != null)
                    {
                        if (at.Id == 3 && ans.Any()) // выбран свободный ответ, но все же есть ответы на вопрос
                            errors.Add(GetErrorStringFor(val.Name)[3]);
                        if (at.Id != 3 && !ans.Any())
                            errors.Add(GetErrorStringFor(val.Name)[0]);
                    }
                    break;               
            }
            base.CheckProperty(context, errors, val);        
        }
    }
}
