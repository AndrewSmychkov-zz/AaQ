using DomainModels.Interface;
using System;
using System.Collections.Generic;

namespace DomainModels.Models
{
    public class Question : IQuestionModel
    {
        private readonly IList<IAnswerModel> _answers;
        public Question()
        {
            _answers = new List<IAnswerModel>();
        }
        public IList<IAnswerModel> Answers
        {
            get
            {
                return _answers;
            }
        }

        public IGlossaryItemModel AnswerType
        {
            get;

            set;
        }

        public Guid Id
        {
            get;

            set;
        }

        public string Text
        {
            get;

            set;
        }
    }
}
