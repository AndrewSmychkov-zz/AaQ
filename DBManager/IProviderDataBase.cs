using System;
using System.Collections.Generic;
using System.Data;

namespace DBManager
{
    public interface IProviderDataBase : IDisposable
    {       
        /// <summary>
        /// Начинаем транзакцию
        /// </summary>
        /// <param name="isolationlevel">уровень изоляции</param>
        void BeginTransaction(IsolationLevel isolationlevel = IsolationLevel.ReadCommitted);
        /// <summary>
        /// Подверждение транзакции
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Откат транзакции
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <returns>массив элементов T</returns>
        IEnumerable<T> ExecuteReader<T>(string nameofprocedure, IDBParameter[] parametrs) where T : class, new();
        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <param name="createmodel">правила разбора IdataReader</param>
        /// <returns>массив элементов T</returns>
        IEnumerable<T> ExecuteReader<T>(string nameofprocedure, IDBParameter[] parametrs, Func<IDataReader, T> createmodel);
        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <returns>экземпляр типа T</returns>
        T ExecuteReaderOne<T>(string nameofprocedure, IDBParameter[] parametrs) where T : class, new();
        /// <summary>
        /// Выполняем процедуру
        /// </summary>
        /// <typeparam name="T">тип возвращаемых элементов</typeparam>
        /// <param name="nameofprocedure">имя процедуры</param>
        /// <param name="parametrs">параметры</param>
        /// <param name="createmodel">правила разбора IdataReader</param>
        /// <returns>экземпляр типа T</returns>
        T ExecuteReaderOne<T>(string nameofprocedure, IDBParameter[] parametrs, Func<IDataReader, T> createmodel) where T : class;
        int ExecuteNonQuery(string nameofprocedure, IDBParameter[] parametrs);
        object ExecuteScalar(string nameofprocedure, IDBParameter[] parametrs);
    }
}
