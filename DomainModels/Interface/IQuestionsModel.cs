using System;
using System.Collections.Generic;

namespace DomainModels.Interface
{
    public interface IQuestionModel
    {
        /// <summary>
        /// Индентификатор
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Текст вопроса
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Тип вопроса
        /// </summary>
        IGlossaryItemModel AnswerType { get; set; }
        /// <summary>
        /// Список ответов на вопрос
        /// </summary>
        IList<IAnswerModel> Answers { get; }
    }
}
