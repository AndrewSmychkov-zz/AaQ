using BusinessLayer.Interface;
using DataAccessLayer;
using DomainModels.Interface;
using DomainModels.Models;
using DomainModels.Validators;
using System.Linq;

namespace BusinessLayer.Managers
{
    sealed class UserManager : IUserManager
    {
        public bool DeleteUser(IUserModel user, string ip)
        {
            if (ProviderFactory.ProviderTicket.CheckTicket(user, ip))
               return ProviderFactory.ProviderUser.DeleteUser(user);
            return false;
        }

        public User LogOn(IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidatePropertys(user, new[] { "Login", "Password" }).Any())
                return ProviderFactory.ProviderUser.LogOn(user,ip);
            return null;
        }

        public bool RegisterUser(IUserModel user)
        {
            if (ValidatorFactory.ValidatorUser.IsValidObject(user))
                return ProviderFactory.ProviderUser.RegisterUser(user);
            return false;
        }
    }
}
