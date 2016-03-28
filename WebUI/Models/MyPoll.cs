using DomainModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class MyPoll
    {      
        public Guid Id
        {
            get;

            set;
        }
        [Display(Name= "Название опросника")]
        public string Name
        {
            get;

            set;
        }
        [Display(Name = "Количество вопросов")]
        public int CountQuestions
        {
            get;
            set;
        }
    }
}