using DomainModels.Interface;
using DomainModels.Models;

namespace BusinessLayer.Interface
{
    public interface IUserManager
    {
        bool DeleteUser(IUserModel user, string ip);
        bool RegisterUser(IUserModel user);
        User LogOn(IUserModel user, string ip);
    }
}
