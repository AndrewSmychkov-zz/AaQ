
namespace DBManager
{
  public  interface IDBParameter
    {
        string ParameterName
        {
            get; set;
        }

        object Value
        {
            get; set;
        }
    }
}
