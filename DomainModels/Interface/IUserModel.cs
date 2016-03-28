using System;

namespace DomainModels.Interface
{
    public interface IUserModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        Guid Id{ get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; set; }
    }
}
