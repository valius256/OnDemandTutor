namespace OnDemandTutor.DataAccess.Models.Exception
{
    public class DataNotFoundException : System.Exception
    {
        public DataNotFoundException(string value) : base(value)
        {
        }
    }
}
