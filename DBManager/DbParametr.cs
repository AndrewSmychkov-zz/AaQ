
namespace DBManager
{
   public sealed class DBParameter: IDBParameter
    {
        public DBParameter(string parameterName, object value)
        {
            ParameterName = parameterName;
            Value = value;
        }

        public string ParameterName
        {
            get; set;
        }
       
        public object Value
        {
            get; set;
        }
    }
}
