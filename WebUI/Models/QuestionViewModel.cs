using DomainModels.Interface;
using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebUI.Validate;

namespace WebUI.Models
{
    public class QuestionViewModel
    {
         [Display(Name = "Ответы")]
        public ICollection<Answer> Answers
        {
            get;
            set;
        }
       
        public byte AnswerTypeId
        {
            get;
            set;
        }
         [Display(Name = "Тип вопроса")]
        public IEnumerable<IGlossaryItemModel> AnswerTypes
        {
            get;
            set;
        }
        public Guid Id
        {
            get;
            set;
        }
        public Guid PackageId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [ValidateByPrimitives("IQuestionModel", "Text",
   ErrorMessage = "{0} должен быть в диапазоне от 1 до 250 символов.")]
        [Display(Name = "Текст вопроса")]
        [DataType(DataType.Text)]
        public string Text
        {
            get;
            set;
        }
    }
}