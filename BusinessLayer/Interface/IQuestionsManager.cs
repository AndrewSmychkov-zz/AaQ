using DomainModels.Interface;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface IQuestionsManager
    {
        bool DeleteQuestions(IQuestionModel question, IUserModel user, string ip);
        IEnumerable<IQuestionModel> GetQuestions(IPackageModel package, IUserModel user, string ip);
        IEnumerable<IQuestionModel> GetMyQuestions(IPackageModel package, IUserModel user, string ip);
        bool SaveQuestions(IQuestionModel question, IPackageModel package, IUserModel user, string ip);
    }
}
