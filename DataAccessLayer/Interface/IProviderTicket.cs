using DomainModels.Interface;

namespace DataAccessLayer.Interface
{
    public interface IProviderTicket
    {
        /// <summary>
        /// проверяем, что пользователь имеет валидный тикет
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="ip_adress">ip-адреса</param>
        /// <returns>true - если проверка прошла успешно</returns>
        bool CheckTicket(IUserModel user, string ip_adress);
    }
}
