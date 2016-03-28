using BusinessLayer.Interface;
using DataAccessLayer;
using System.Collections.Generic;
using DomainModels.Interface;
using DomainModels.Models;
using DomainModels.Validators;
using System.Linq;

namespace BusinessLayer.Managers
{
    sealed class GlossaryManager : IGlossaryManager
    {
        public IEnumerable<IGlossaryItemModel> GetAllAnswerType(IUserModel user,string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderAnswerType.AllObjects;
            return new GlossaryItem[0];
        }

        public IEnumerable<IGlossaryItemModel> GetlAllMyAnswerType(IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderMyAnswerType.AllObjects;
            return new GlossaryItem[0];
        }
    }
}
