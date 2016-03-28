using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataAccessLayer.Provider
{
    public static class ExtensionMethodForDBProperties
    {       
        private static void CopyObject(IDataRecord model, object copyesModel, string[] exception)
        {
            if (copyesModel == null || model == null)
                return;
            foreach (var p in copyesModel.GetType().GetProperties().Where(x => x.CanRead && !exception.Select(y => y.ToLower()).Contains(x.Name.ToLower())).ToArray())
            {
                for (int i = 0; i < model.FieldCount; i++)
                {
                    if (Compare(model.GetName(i), p))
                    {
                        copyesModel.GetType().GetProperty(p.Name).SetValue(copyesModel, model[i].GetType() == typeof(System.DBNull) ? null : model[i], null);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// сравниваем копируемые свойства
        /// </summary>
        /// <param name="p">что</param>
        /// <param name="p2">куда</param>
        private static bool Compare(string p, PropertyInfo p2)
        {
            return (String.Equals(p, p2.Name, StringComparison.CurrentCultureIgnoreCase));                         
        }

        public static void DBFillObject(this IDataRecord model, object copyesModel, string[] exception)
        {
            CopyObject(model, copyesModel, exception);
        }

        public static T CreateObjectFromDb<T>(this IDataRecord model) where T : class, new()
        {
            return model.CreateObjectFromDb<T>(new[] { "" });
        }

        public static T CreateObjectFromDb<T>(this IDataRecord model, string[] exception) where T : class, new()
        {
            if (model == null)
                return null;

            var creatingOfModel = new T();
            model.DBFillObject(creatingOfModel, exception);

            return creatingOfModel;
        }
    }
}
