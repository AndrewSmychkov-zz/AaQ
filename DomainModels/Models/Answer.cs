using DomainModels.Interface;
using System;

namespace DomainModels.Models
{
    /// <summary>
    /// Ответы на вопрос
    /// </summary>
    public class Answer : IAnswerModel
    {
        /// <summary>
        /// Идентификтор
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// Ответ
        /// </summary>
        public string Name
        {
            get;

            set;
        }
    }
}
