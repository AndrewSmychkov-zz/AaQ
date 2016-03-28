using DataAccessLayer.Interface;
using System;
using DomainModels.Interface;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Provider
{
    internal sealed class ProviderTicket : IProviderTicket
    {
        private readonly string _connection;
        public ProviderTicket(string connection)
        {
            _connection = connection;
        }
        public bool CheckTicket(IUserModel user, string ip_adress)
        {
            Guid g;
            if (!Guid.TryParse(user.Password, out g))
                return false;
            SqlParameter[] lp = new[]
          {              
                new SqlParameter("@userid", user.Id),
                new SqlParameter("@ticket", g),
                new SqlParameter("@ip_adress", ip_adress),
                new SqlParameter("@current1",DateTime.UtcNow.AddHours(1))
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
    }
}
