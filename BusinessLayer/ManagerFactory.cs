using System;
using BusinessLayer.Interface;
using BusinessLayer.Managers;

namespace BusinessLayer
{
    public static class ManagerFactory
    {
        #region private
        private static readonly Lazy<IUserManager> _manageruser
        = new Lazy<IUserManager>(() => new UserManager());

        private static readonly Lazy<IGlossaryManager> _managerglossary
      = new Lazy<IGlossaryManager>(() => new GlossaryManager());

        private static readonly Lazy<IAnswerManager> _manageranswer
        = new Lazy<IAnswerManager>(() => new AnswerManager());

        private static readonly Lazy<IPackageManager> _managerpackage
       = new Lazy<IPackageManager>(() => new PackageManager());

        private static readonly Lazy<IQuestionsManager> _managerquestion
      = new Lazy<IQuestionsManager>(() => new QuestionManager());
        #endregion

        #region Properties
        public static IUserManager UserManager
        {
            get { return _manageruser.Value; }
        }

        public static IGlossaryManager GlossaryManager
        {
            get { return _managerglossary.Value; }
        }

        public static IAnswerManager AnswerManager
        {
            get { return _manageranswer.Value; }
        }

        public static IPackageManager PackageManager
        {
            get { return _managerpackage.Value; }
        }
        public static IQuestionsManager QuestionManager
        {
            get { return _managerquestion.Value; }
        }
        #endregion
    }
}
