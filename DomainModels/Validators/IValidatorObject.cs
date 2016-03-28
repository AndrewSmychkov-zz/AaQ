using System.Collections.Generic;

namespace DomainModels.Validators
{
    public interface IValidatorObject<in T>: IValidatorPrimitives
    {

        /// <summary>
        /// валидируем все свойства которые смогли прочитать
        /// </summary>
        /// <param name="context">контекст</param>
        /// <returns>ассоциативный массив ошибок. Где ключ - название свойства, занчение - массив ошибок данного свойства</returns>
        IDictionary<string, IEnumerable<string>> ValidateObject(T context);

        /// <summary>
        /// валидация конкретного свойств контекста
        /// </summary>
        /// <param name="context">контекст</param>
        /// <param name="propertyName">свойства</param>
        /// <returns>массив ошибок данного свойства</returns>
        IEnumerable<string> ValidateProperty(T context, string propertyName);

        /// <summary>
        /// валидация нескольких свойств контекста
        /// </summary>
        /// <param name="context">контекста</param>
        /// <param name="propertyNames">массив свойств</param>
        /// <returns>ассоциативный массив ошибок. Где ключ - название свойства, занчение - массив ошибок данного свойства</returns>
        IDictionary<string, IEnumerable<string>> ValidatePropertys(T context, IEnumerable<string> propertyNames);

        /// <summary>
        /// валидирует объект
        /// </summary>
        /// <param name="context">объект</param>
        /// <returns>возвращает true, если объект не содержит ошибок</returns>
        bool IsValidObject(T context);       
    }
}
