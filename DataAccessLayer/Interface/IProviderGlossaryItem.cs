using DomainModels.Interface;
using System.Collections.Generic;


namespace DataAccessLayer.Interface
{
    public  interface IProviderGlossaryItem
    {
        /// <summary>
        /// Получаем все объекты лениновой иниицилизацией
        /// </summary>
        /// <typeparam name="T">Объект, который статичен, никогда не изменяется пользователем</typeparam>
        IEnumerable<IGlossaryItemModel> AllObjects
        {
            get;
        }
        /// <summary>
        /// сбрасываем объекты <see cref = "AllObjects" />
        /// </summary>
        void RefreshCollection();
    }
}
