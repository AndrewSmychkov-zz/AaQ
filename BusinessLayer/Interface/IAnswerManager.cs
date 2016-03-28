using DomainModels.Interface;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface IAnswerManager
    {
        bool DeleteAnswer(IAnswerModel answer, IUserModel user, string ip);
        IEnumerable<IAnswerModel> GetAnswer(IQuestionModel question, IUserModel user, string ip);
        IEnumerable<IAnswerModel> GetMyAnswer(IQuestionModel question, IUserModel user, string ip);
        bool SaveAnswer(IAnswerModel answer, IQuestionModel question, IUserModel user, string ip);
    }
}
