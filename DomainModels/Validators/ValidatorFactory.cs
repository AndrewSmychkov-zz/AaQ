using DomainModels.Interface;
using System;

namespace DomainModels.Validators
{
    public static class ValidatorFactory
    {
        #region Private fields      

        private static readonly Lazy<IValidatorObject<IUserModel>> _validatoruser
          = new Lazy<IValidatorObject<IUserModel>>(() => new ValidatorUser());      

        private static readonly Lazy<IValidatorObject<IAnswerModel>> _validatoranswer
        = new Lazy<IValidatorObject<IAnswerModel>>(() => new ValidatorAnswer());

        private static readonly Lazy<IValidatorObject<IPackageModel>> _validatorpackage
       = new Lazy<IValidatorObject<IPackageModel>>(() => new ValidatorPackage());

        private static readonly Lazy<IValidatorObject<IQuestionModel>> _validatorquestion
      = new Lazy<IValidatorObject<IQuestionModel>>(() => new ValidatorQuestion());

        #endregion

        #region properties       

        public static IValidatorObject<IUserModel> ValidatorUser
        {
            get { return _validatoruser.Value; }
        }

        public static IValidatorObject<IAnswerModel> ValidatorAnswer
        {
            get { return _validatoranswer.Value; }
        }

        public static IValidatorObject<IPackageModel> ValidatorPackage
        {
            get { return _validatorpackage.Value; }
        }

        public static IValidatorObject<IQuestionModel> ValidatorQuestion
        {
            get { return _validatorquestion.Value; }
        }

        #endregion

        #region Method
        public static IValidatorPrimitives GetValidatorByInterfaceName(string nameofinterface)
        {
            if (nameofinterface == "IUserModel")
            {
                return _validatoruser.Value;
            }
            if (nameofinterface == "IAnswerModel")
            {
                return _validatoranswer.Value;
            }
            if (nameofinterface == "IPackageModel")
            {
                return _validatorpackage.Value;
            }
            if (nameofinterface == "IQuestionModel")
            {
                return _validatorquestion.Value;
            }
            return null;
        }
        #endregion
    }
}
