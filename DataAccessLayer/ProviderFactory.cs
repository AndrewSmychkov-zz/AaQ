using DataAccessLayer.Interface;
using DataAccessLayer.Provider;
using System;


namespace DataAccessLayer
{
    public static class ProviderFactory
    {
        //!A54RDYTR
        private static string user = "appuser";
        private static string password = "!A54RDYTR";

        private static string connectionstring = string.Format("data source=smychkov-mssql.database.windows.net;initial catalog=AaQ_test;Pooling=true;Min Pool Size=1;Max Pool Size=10;user id={0};password={1};multipleactiveresultsets=True", user, password);
     
        #region Private fields
        private static readonly Lazy<IProviderUser> _provideruser
        = new Lazy<IProviderUser>(() => new ProviderUser(connectionstring));

        private static readonly Lazy<IProviderAnswer> _provideranswer
        = new Lazy<IProviderAnswer>(() => new ProviderAnswer(connectionstring));

        private static readonly Lazy<IProviderQuestion> _providerquestion
      = new Lazy<IProviderQuestion>(() => new ProviderQuestion(connectionstring));

        private static readonly Lazy<IProviderPackage> _providerpackage
     = new Lazy<IProviderPackage>(() => new ProviderPackage(connectionstring));

        private static readonly Lazy<IProviderTicket> _providerticket
       = new Lazy<IProviderTicket>(() => new ProviderTicket(connectionstring));

        private static readonly Lazy<IProviderGlossaryItem> _provideranswertype
      = new Lazy<IProviderGlossaryItem>(() => new ProviderGlossaryItem(connectionstring, "GetTypeOfAnswer"));

        private static readonly Lazy<IProviderGlossaryItem> _providermyanswertype
     = new Lazy<IProviderGlossaryItem>(() => new ProviderGlossaryItem(connectionstring, "GetTypeOfMyAnswer"));
        #endregion

        #region Properties
        public static IProviderUser ProviderUser
        {
            get
            {
                return _provideruser.Value;
            }
        }
        public static IProviderPackage ProviderPackage
        {
            get
            {
                return _providerpackage.Value;
            }
        }
        public static IProviderAnswer ProviderAnswer
        {
            get
            {
                return _provideranswer.Value;
            }
        }
        public static IProviderQuestion ProviderQuestion
        {
            get
            {
                return _providerquestion.Value;
            }
        }
        public static IProviderTicket ProviderTicket
        {
            get
            {
                return _providerticket.Value;
            }
        }
        public static IProviderGlossaryItem ProviderAnswerType
        {
            get
            {
                return _provideranswertype.Value;
            }
        }
        public static IProviderGlossaryItem ProviderMyAnswerType
        {
            get
            {
                return _providermyanswertype.Value;
            }
        }
        #endregion
    }
}
