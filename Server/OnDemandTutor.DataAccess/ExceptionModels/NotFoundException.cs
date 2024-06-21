namespace OnDemandTutor.DataAccess.ExceptionModels;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base(message)
    {
    }
}