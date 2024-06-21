namespace OnDemandTutor.DataAccess.ExceptionModels;

public class DataNotFoundException : Exception
{
    public DataNotFoundException(string value) : base(value)
    {
    }
}