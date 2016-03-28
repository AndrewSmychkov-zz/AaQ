using System;

namespace DomainModels.Interface
{
    public interface IAnswerModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Ответ
        /// </summary>
        string Name { get; set; }
    }
}
