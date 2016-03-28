using DomainModels.Interface;
using System;

namespace DomainModels.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IUserModel
    {       
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id
        {
            get;

            set;
        }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login
        {
            get;

            set;
        }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }
}
