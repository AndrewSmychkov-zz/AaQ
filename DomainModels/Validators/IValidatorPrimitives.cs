using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Validators
{
    public interface IValidatorPrimitives
    {
        /// <summary>
        /// Проверка примитивного свойства
        /// </summary>
        /// <param name="value">занчение</param>
        /// <param name="propertyName">имя свойства/правила</param>
        /// <returns>список ошибок</returns>
        IEnumerable<string> ValidateProperty(object value, string propertyName);

        /// <summary>
        /// Получить все сообщения об ошибке для конкретного поля
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string[] GetErrorStringFor(string propertyName);
    }
}
