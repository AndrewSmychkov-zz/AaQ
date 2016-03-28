using DomainModels.Interface;
using System.Collections.Generic;

namespace DataAccessLayer.Interface
{
    public interface IProviderQuestion
    {
        bool SaveQuestion(IQuestionModel question, IPackageModel package, IUserModel user);

        bool DeleteQuestion(IQuestionModel question, IUserModel user);

        IEnumerable<IQuestionModel> GetQuestion(IPackageModel package);

        IEnumerable<IQuestionModel> GetMyQuestion(IPackageModel package, IUserModel user);
    }
}
