using DomainModels.Interface;

namespace DomainModels.Models
{
   public class GlossaryItem : IGlossaryItemModel
    {

        private readonly byte _id;
        private readonly string _name;
        private readonly string _description;
        public GlossaryItem(byte id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public byte Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
