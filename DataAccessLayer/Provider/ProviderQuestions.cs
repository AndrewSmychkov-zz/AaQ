using DataAccessLayer.Interface;
using DomainModels.Interface;
using DomainModels.Models;
using DomainModels.Validators;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer.Provider
{
    sealed class ProviderQuestion : IProviderQuestion
    {
        private readonly string _connection;
        public ProviderQuestion(string connection)
        {
            _connection = connection;
        }

        public bool DeleteQuestion(IQuestionModel question, IUserModel user)
        {
            if (ValidatorFactory.ValidatorQuestion.ValidateProperty(question, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@id", question.Id),
                new SqlParameter("@userid", user.Id)
            };
            using (var db = new SqlCommand("DeleteQuestion", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    return (int)db.ExecuteScalar() == 1;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }


        private Question CreateQuestion(IDataReader datareader)
        {
            return new Question { Id = datareader.GetGuid(0), Text = datareader.GetString(1), AnswerType = ProviderFactory.ProviderAnswerType.AllObjects.FirstOrDefault(x => x.Id == datareader.GetByte(2)) };
        }

        public IEnumerable<IQuestionModel> GetMyQuestion(IPackageModel package, IUserModel user)
        {
            if (ValidatorFactory.ValidatorPackage.ValidateProperty(package, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return new Question[0]; 
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@user_id", user.Id),
                new SqlParameter("@package_id", package.Id)
            };
            using (var db = new SqlCommand("GetQuestionsByMyPackage", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IQuestionModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(CreateQuestion(dataReader));
                    }
                    dataReader.Close();
                    return allList;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }

        public IEnumerable<IQuestionModel> GetQuestion(IPackageModel package)
        {
            if (ValidatorFactory.ValidatorPackage.ValidateProperty(package, "Id").Any())
                return new Question[0];
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@package_id", package.Id)
            };
            using (var db = new SqlCommand("GetQuestionsByPackage", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IQuestionModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(CreateQuestion(dataReader));
                    }
                    dataReader.Close();
                    return allList;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }

        public bool SaveQuestion(IQuestionModel question, IPackageModel package, IUserModel user)
        {
            if (ValidatorFactory.ValidatorQuestion.ValidatePropertys(question, new[] { "Id", "Text" }).Any() || ValidatorFactory.ValidatorPackage.ValidateProperty(package, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@id", question.Id) ,
                new SqlParameter("@userid", user.Id),
                new SqlParameter("@text", question.Text),
                new SqlParameter("@package_id", package.Id),
                new SqlParameter("@answertype_id", question.AnswerType.Id)
            };
            using (var db = new SqlCommand("SaveQuestions", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    return (int)db.ExecuteScalar() == 1;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }
    }
}
