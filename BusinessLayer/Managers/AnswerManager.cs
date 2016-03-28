using BusinessLayer.Interface;
using DomainModels.Validators;
using System.Collections.Generic;
using DomainModels.Interface;
using DataAccessLayer;
using DomainModels.Models;
using System.Linq;

namespace BusinessLayer.Managers
{
    sealed class AnswerManager : IAnswerManager
    {
        public bool DeleteAnswer(IAnswerModel answer, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderAnswer.DeleteAnswer(answer, user);
            return false;
        }

        public IEnumerable<IAnswerModel> GetAnswer(IQuestionModel question, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderAnswer.GetAnswer(question);
            return new Answer[0];
        }

        public IEnumerable<IAnswerModel> GetMyAnswer(IQuestionModel question, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderAnswer.GetMyAnswer(question,user);
            return new Answer[0];
        }

        public bool SaveAnswer(IAnswerModel answer, IQuestionModel question, IUserModel user, string ip)
        {
            if (ValidatorFactory.ValidatorAnswer.IsValidObject(answer) && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderAnswer.SaveAnswer(answer, question, user);
            return false;
        }
    }
}
