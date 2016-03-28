using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace DBManager
{
    public sealed class ProviderDataBase : IProviderDataBase
    {
        private readonly IDbCommand _command;
        public ProviderDataBase(IDbCommand command)
        {
            _command = command;
            _command.CommandType = CommandType.StoredProcedure;
        }

        #region приватные методы
        /// <summary>
        /// подготовка к выполнению процедуры
        /// </summary>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">парметры процедуры</param>
        private void CommandLoad(ref string nameofprocedure, IDBParameter[] parametrs)
        {
            OpenConnection();
            _command.CommandText = nameofprocedure;
            _command.Parameters.Clear();
            for (var i = 0; i < parametrs.Length; i++)
            {
                var param = _command.CreateParameter();
                param.ParameterName = parametrs[i].ParameterName;
                param.Value = parametrs[i].Value;
                _command.Parameters.Add(param);
            }
            _command.Prepare();
        }

        private void OpenConnection()
        {
            if (_command.Connection == null)
                throw new Exception("В DbCommand не найдено соединение. Connection == null");
            if (_command.Connection.State != ConnectionState.Open)
            {
                _command.Connection.Close();
                _command.Connection.Open();
            }
        }
        #endregion
        /// <summary>
        /// Начинаем транзакцию
        /// </summary>
        public void BeginTransaction(IsolationLevel isolationlevel = IsolationLevel.ReadCommitted)
        {
            if (_command == null)
                return;
            OpenConnection();
            _command.Transaction = _command.Connection.BeginTransaction(isolationlevel);

        }
        /// <summary>
        /// Подверждение транзакции
        /// </summary>
        public void CommitTransaction()
        {
            if (_command == null)
                return;
            if (_command.Transaction != null)
            {
                _command.Transaction.Commit();
            }
        }
        /// <summary>
        /// Откат транзакции
        /// </summary>
        public void RollbackTransaction()
        {
            if (_command == null)
                return;
            if (_command.Transaction != null)
            {
                _command.Transaction.Rollback();
            }
        }

        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <returns>массив элементов T</returns>
        public IEnumerable<T> ExecuteReader<T>(string nameofprocedure, IDBParameter[] parametrs) where T : class, new()
        {
            var allList = new Collection<T>();
            if (_command == null)
                return allList;
            CommandLoad(ref nameofprocedure, parametrs);
            var dataReader = _command.ExecuteReader();
            while (dataReader.Read())
            {
                allList.Add(dataReader.CreateObjectFromDb<T>());
            }
            dataReader.Close();
            return allList;
        }
        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <param name="createmodel">правила разбора IdataReader</param>
        /// <returns>массив элементов T</returns>
        public IEnumerable<T> ExecuteReader<T>(string nameofprocedure, IDBParameter[] parametrs, Func<IDataReader, T> createmodel)
        {
            var allList = new Collection<T>();
            if (_command == null)
                return allList;
            CommandLoad(ref nameofprocedure, parametrs);
            var dataReader = _command.ExecuteReader();
            while (dataReader.Read())
            {
                allList.Add(createmodel.Invoke(dataReader));
            }
            dataReader.Close();
            return allList;
        }
        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <returns>экземпляр типа T</returns>
        public T ExecuteReaderOne<T>(string nameofprocedure, IDBParameter[] parametrs) where T : class, new()
        {
            if (_command == null)
                return null;
            CommandLoad(ref nameofprocedure, parametrs);
            T model = null;
            var dataReader = _command.ExecuteReader(CommandBehavior.SingleRow);
            if (dataReader.Read())
                model = dataReader.CreateObjectFromDb<T>();
            dataReader.Close();
            return model;
        }

        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <param name="createmodel">правила разбора IdataReader</param>
        /// <returns>экземпляр типа T</returns>
        public T ExecuteReaderOne<T>(string nameofprocedure, IDBParameter[] parametrs, Func<IDataReader, T> createmodel) where T : class
        {
            if (_command == null)
                return null;
            CommandLoad(ref nameofprocedure, parametrs);
            var dataReader = _command.ExecuteReader(CommandBehavior.SingleRow);
            T model = null;
            if (dataReader.Read())
                model = createmodel.Invoke(dataReader);
            dataReader.Close();
            return model;
        }

        public int ExecuteNonQuery(string nameofprocedure, IDBParameter[] parametrs)
        {
            if (_command == null)
                return 0;
            CommandLoad(ref nameofprocedure, parametrs);
            return _command.ExecuteNonQuery();
        }
        public object ExecuteScalar(string nameofprocedure, IDBParameter[] parametrs)
        {
            if (_command == null)
                return null;
            CommandLoad(ref nameofprocedure, parametrs);
            return _command.ExecuteScalar();
        }
        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов


        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CommitTransaction();
                    _command.Dispose();
                }

                // освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        ~ProviderDataBase()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(false);
        }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
