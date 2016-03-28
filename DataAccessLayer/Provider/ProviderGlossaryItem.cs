using DataAccessLayer.Interface;
using DomainModels.Interface;
using DomainModels.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Provider
{
    sealed class ProviderGlossaryItem : IProviderGlossaryItem
    {
        private readonly string _nameofprocedure;
        private readonly string _connection;

        volatile private IEnumerable<IGlossaryItemModel> _allobject;

        public IEnumerable<IGlossaryItemModel> AllObjects
        {
            get
            {
                return _allobject ?? (_allobject = GetAll());
            }
        }
        private IEnumerable<IGlossaryItemModel> GetAll()
        {
            using (var db = new SqlCommand(_nameofprocedure, new SqlConnection(_connection)))
            {
                try
                {
                    db.Connection.Open();
                    db.CommandType = CommandType.StoredProcedure;
                    var allList = new Collection<IGlossaryItemModel>();
                    var dataReader = db.ExecuteReader();
                    while (dataReader.Read())
                    {
                        allList.Add(CreateGlossaryItem(dataReader));
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
        private GlossaryItem CreateGlossaryItem(IDataReader dataReader)
        {
            return new GlossaryItem(dataReader.GetByte(0), dataReader.GetString(1), dataReader.GetString(2));
        }

        public void RefreshCollection()
        {
            _allobject = GetAll();
        }
        /// <summary>
        /// класс по статичным спискам
        /// </summary>
        /// <param name="_provider">соеденение</param>
        /// <param name="nameofprocedure">имя процедуры которая выгружает статичный список</param>        
        public ProviderGlossaryItem(string connetion, string nameofprocedure)
        {
            _nameofprocedure = nameofprocedure;
            _connection = connetion;
        }

    }
}
