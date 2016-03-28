using BusinessLayer.Interface;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Interface;
using DataAccessLayer;
using DomainModels.Validators;
using DomainModels.Models;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    sealed class PackageManager : IPackageManager
    {
        public bool DeletePackage(IPackageModel package, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderPackage.DeletePackage(package, user);
            return false;
        }

        public IEnumerable<IPackageModel> GetMyPackage(IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
            {
                var ps= ProviderFactory.ProviderPackage.GetMyPackage(user);
                Parallel.ForEach(ps, x =>
                 {
                     foreach (var q in ProviderFactory.ProviderQuestion.GetMyQuestion(x, user))
                     {
                         x.Questions.Add(q);
                     }
                 });
                return ps;
            }
            return new Package[0];
        }

        public IEnumerable<IPackageModel> GetPackage(IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderPackage.GetPackage(user);
            return new Package[0];
        }

        public bool SavePackage(IPackageModel package, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorPackage.ValidatePropertys(package, new[] {"Id", "Name" }).Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderPackage.SavePackage(package, user);
            return false;
        }
    }
}
