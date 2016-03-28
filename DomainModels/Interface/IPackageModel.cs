using System;
using System.Collections.Generic;

namespace DomainModels.Interface
{
    public  interface IPackageModel
    {/// <summary>
    /// Идентификатор
    /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; } 
        /// <summary>
        /// Список вопросов 
        /// </summary>
        IList<IQuestionModel> Questions { get; }
    }
}
