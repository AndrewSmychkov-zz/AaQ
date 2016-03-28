using DomainModels.Interface;
using System.Collections.Generic;


namespace BusinessLayer.Interface
{
   public interface IGlossaryManager
    {
        IEnumerable<IGlossaryItemModel> GetAllAnswerType(IUserModel user , string ip);
        IEnumerable<IGlossaryItemModel> GetlAllMyAnswerType(IUserModel user, string ip);
    }
}
