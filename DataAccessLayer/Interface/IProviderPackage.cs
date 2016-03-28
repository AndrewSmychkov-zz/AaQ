using DomainModels.Interface;
using System.Collections.Generic;

namespace DataAccessLayer.Interface
{
    public interface IProviderPackage
    {
        bool SavePackage(IPackageModel package, IUserModel user);

        bool DeletePackage(IPackageModel package, IUserModel user);

        IEnumerable<IPackageModel> GetPackage(IUserModel user);

        IEnumerable<IPackageModel> GetMyPackage(IUserModel user);
    }
}
