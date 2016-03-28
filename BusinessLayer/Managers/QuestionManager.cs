using System.Threading.Tasks;
using System.Collections.Generic;
using BusinessLayer.Interface;
using DomainModels.Interface;
using DataAccessLayer;
using DomainModels.Models;
using DomainModels.Validators;
using System.Linq;

namespace BusinessLayer.Managers
{
    sealed class QuestionManager : IQuestionsManager
    {
        public bool DeleteQuestions(IQuestionModel question, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderQuestion.DeleteQuestion(question, user);
            return false;
        }

        public IEnumerable<IQuestionModel> GetMyQuestions(IPackageModel package, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
            {
               return ProviderFactory.ProviderQuestion.GetMyQuestion(package, user);           
            }
            return new Question[0];
        }

        public IEnumerable<IQuestionModel> GetQuestions(IPackageModel package, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
                return ProviderFactory.ProviderQuestion.GetQuestion(package);
            return new Question[0];
        }

        public bool SaveQuestions(IQuestionModel question, IPackageModel package, IUserModel user, string ip)
        {
            if (!ValidatorFactory.ValidatorQuestion.ValidatePropertys(question, new[] { "Id", "Text" }).Any()
                && !ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any() 
                && !ValidatorFactory.ValidatorPackage.ValidateProperty(package, "Id").Any()
                && ProviderFactory.ProviderTicket.CheckTicket(user, ip))
            {
                return ProviderFactory.ProviderQuestion.SaveQuestion(question, package, user);                
            }
            return false;
        }
    }
}
