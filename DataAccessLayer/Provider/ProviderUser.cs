using DataAccessLayer.Interface;
using DomainModels.Interface;
using DomainModels.Models;
using System.Data;
using DomainModels.Validators;
using System.Linq;
using System.Data.SqlClient;

namespace DataAccessLayer.Provider
{
    internal sealed class ProviderUser : IProviderUser
    {
        private readonly string _connection;
        public ProviderUser(string connection)
        {
            _connection = connection;
        }
        public bool DeleteUser(IUserModel user)
        {
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@userid",user.Id)
            };

            using (var db = new SqlCommand("CheckTicket", new SqlConnection(_connection)))
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

        public User LogOn(IUserModel user, string ipadress)
        {
            if (ValidatorFactory.ValidatorUser.ValidatePropertys(user, new[] { "Login", "Password" }).Any())
                return null;
            SqlParameter[] lp = new[]
           {
                new SqlParameter("@login", user.Login) { SqlDbType=SqlDbType.NVarChar, Size=50},
                new SqlParameter("@password", user.Password){ SqlDbType=SqlDbType.NVarChar, Size=8000},
                new SqlParameter("@ip_adress", ipadress){ SqlDbType=SqlDbType.VarChar, Size=50}
            };
            using (var db = new SqlCommand("LogOnUser", new SqlConnection(_connection)))
            {
                try
                {
                    User model = null;
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var dataReader = db.ExecuteReader(CommandBehavior.SingleRow);
                    if (dataReader.Read())
                    {
                        model = dataReader.CreateObjectFromDb<User>(new[] { "Password" });
                        model.Password = dataReader.GetGuid(2).ToString();
                    }
                    dataReader.Close();
                    return model;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }

        public bool RegisterUser(IUserModel user)
        {
            if (!ValidatorFactory.ValidatorUser.IsValidObject(user))
                return false;
            SqlParameter[] lp = new[]
           {
                new SqlParameter("@id", user.Id),
                new SqlParameter("@login", user.Login),
                new SqlParameter("@password", user.Password)
            };
            using (var db = new SqlCommand("RegUser", new SqlConnection(_connection)))
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
