using DomainModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebUI.Validate;

namespace WebUI.Models
{
    public class EditPoll 
    {
        public Guid Id
        {
            get;
            set;            
        }
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [ValidateByPrimitives("IPackageModel", "Name",
 ErrorMessage = "{0} должен быть в диапазоне от 1 до 250 символов.")]
        [Display(Name = "Текст вопроса")]
        [DataType(DataType.Text)]
        public string Name
        {
            get;
            set;
        }

        //public IList<IQuestionModel> Questions
        //{
        //    get;
        //    set;
        //}
    }
}