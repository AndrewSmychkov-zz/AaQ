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
    sealed class ProviderAnswer : IProviderAnswer
    {
        private readonly string _connection;
        public ProviderAnswer(string connection)
        {
            _connection = connection;
        }

        public bool DeleteAnswer(IAnswerModel answer, IUserModel user)
        {
            if (ValidatorFactory.ValidatorAnswer.ValidateProperty(answer, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@answerid", answer.Id),
                new SqlParameter("@userid", user.Id)
            };
            using (var db = new SqlCommand("DeleteAnswer", new SqlConnection(_connection)))
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

        public IEnumerable<IAnswerModel> GetAnswer(IQuestionModel question)
        {
            if (ValidatorFactory.ValidatorQuestion.ValidateProperty(question, "Id").Any())
                return new Answer[0]; 
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@question_id", question.Id)
            };
            using (var db = new SqlCommand("GetAnswer", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IAnswerModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(dataReader.CreateObjectFromDb<Answer>());
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

        public IEnumerable<IAnswerModel> GetMyAnswer(IQuestionModel question, IUserModel user)
        {
            if (ValidatorFactory.ValidatorQuestion.ValidateProperty(question, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return new Answer[0];
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@userid", user.Id),
                new SqlParameter("@question_id", question.Id)
            };
            using (var db = new SqlCommand("GetAnswerForMyQuestion", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IAnswerModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(dataReader.CreateObjectFromDb<Answer>());
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

        public bool SaveAnswer(IAnswerModel answer, IQuestionModel question, IUserModel user)
        {
            if (!ValidatorFactory.ValidatorAnswer.IsValidObject(answer) || ValidatorFactory.ValidatorQuestion.ValidateProperty(question, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@id", answer.Id) ,
                new SqlParameter("@userid", user.Id),
                new SqlParameter("@text", answer.Name),
                new SqlParameter("@question_id", question.Id)
            };
            using (var db = new SqlCommand("SaveAnswer", new SqlConnection(_connection)))
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
