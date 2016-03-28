using DomainModels.Interface;
using DomainModels.Models;


namespace DataAccessLayer.Interface
{
    public interface IProviderUser
    {
        /// <summary>
        /// регистрация новго пользователя
        /// </summary>
        /// <param name="user">новый пользователь</param>
        /// <returns>true- если сохранение прошло успешно</returns>
        bool RegisterUser(IUserModel user);
        /// <summary>
        /// авторизация пользователя
        /// </summary>     
        /// <param name="ipadress">адрес с которого идет подключение</param>
        /// <returns>профайл авторизационного пользоватлея</returns>
        User LogOn(IUserModel user, string ipadress);
        /// <summary>
        /// удаление пользователя
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <returns>true - удаление прошло успешно</returns>
        bool DeleteUser(IUserModel user);
       
    }
}
