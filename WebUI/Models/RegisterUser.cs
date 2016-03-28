using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebUI.Validate;

namespace WebUI.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [ValidateByPrimitives("IUserModel", "Login",
     ErrorMessage = "Поле {0} содержит меньше 2-х символов.")]
        [Display(Name = "Логин")]
        [DataType(DataType.Text)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [ValidateByPrimitives("IUserModel", "Password",
     ErrorMessage = "Поле {0} содержит меньше 2-х символов.")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}