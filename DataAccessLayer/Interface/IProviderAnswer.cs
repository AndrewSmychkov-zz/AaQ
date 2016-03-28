using DomainModels.Interface;
using System.Collections.Generic;

namespace DataAccessLayer.Interface
{
    public interface IProviderAnswer
    {
        bool SaveAnswer(IAnswerModel answer, IQuestionModel question, IUserModel user);

        bool DeleteAnswer(IAnswerModel answer, IUserModel user);

        IEnumerable<IAnswerModel> GetAnswer(IQuestionModel question);

        IEnumerable<IAnswerModel> GetMyAnswer(IQuestionModel question, IUserModel user);
    }
}
