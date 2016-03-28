using DomainModels.Interface;
using System;
using System.Collections.Generic;

namespace DomainModels.Models
{/// <summary>
/// Опросник
/// </summary>
    public class Package : IPackageModel
    {
        private readonly IList<IQuestionModel> _questions;
        public Package()
        {
            _questions = new List<IQuestionModel>();
        }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id
        {
            get;

            set;
        }
        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get;

            set;
        }

        public IList<IQuestionModel> Questions
        {
            get
            {
                return _questions;
            }
        }
    }
}
