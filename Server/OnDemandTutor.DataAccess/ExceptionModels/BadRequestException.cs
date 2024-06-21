namespace OnDemandTutor.DataAccess.ExceptionModels;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string message) : base(message)
    {
    }
}