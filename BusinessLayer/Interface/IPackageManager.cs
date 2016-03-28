using System.Collections.Generic;
using DomainModels.Interface;

namespace BusinessLayer.Interface
{
   public interface IPackageManager
    {
        bool DeletePackage(IPackageModel package, IUserModel user, string ip);
        bool SavePackage(IPackageModel package, IUserModel user, string ip);
        IEnumerable<IPackageModel> GetPackage(IUserModel user, string ip);
        IEnumerable<IPackageModel> GetMyPackage(IUserModel user, string ip);
    }
}
