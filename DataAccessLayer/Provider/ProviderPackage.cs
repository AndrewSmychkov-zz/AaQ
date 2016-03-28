using DataAccessLayer.Interface;
using System.Collections.Generic;
using DomainModels.Interface;
using System.Data;
using DomainModels.Validators;
using System.Linq;
using DomainModels.Models;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace DataAccessLayer.Provider
{
    sealed class ProviderPackage : IProviderPackage
    {
        private readonly string _connection;
        public ProviderPackage(string connection)
        {
            _connection = connection;
        }

        public bool DeletePackage(IPackageModel package, IUserModel user)
        {
            if (ValidatorFactory.ValidatorPackage.ValidateProperty(package, "Id").Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@id", package.Id),
                new SqlParameter("@userid", user.Id)
            };
            using (var db = new SqlCommand("DeletePackage", new SqlConnection(_connection)))
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

        public IEnumerable<IPackageModel> GetMyPackage(IUserModel user)
        {
            if (ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return new Package[0];
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@userid", user.Id)
            };
            using (var db = new SqlCommand("GetMyPackage", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IPackageModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(dataReader.CreateObjectFromDb<Package>());
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

        public IEnumerable<IPackageModel> GetPackage(IUserModel user)
        {
            if (ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return new Package[0];
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@userid", user.Id)
            };
            using (var db = new SqlCommand("GetPackage", new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    db.Parameters.AddRange(lp);
                    var allList = new Collection<IPackageModel>();

                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(dataReader.CreateObjectFromDb<Package>());
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

        public bool SavePackage(IPackageModel package, IUserModel user)
        {
            if (ValidatorFactory.ValidatorPackage.ValidatePropertys(package, new[] { "Id", "Name" }).Any() || ValidatorFactory.ValidatorUser.ValidateProperty(user, "Id").Any())
                return false;
            SqlParameter[] lp = new[]
            {
                new SqlParameter("@id", package.Id) ,
                new SqlParameter("@userid", user.Id),
                new SqlParameter("@name", package.Name)
            };
            using (var db = new SqlCommand("SavePackage", new SqlConnection(_connection)))
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
