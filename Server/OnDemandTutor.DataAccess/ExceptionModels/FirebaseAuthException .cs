namespace OnDemandTutor.DataAccess.ExceptionModels;

public class FirebaseAuthException : Exception
{
    public FirebaseAuthException(string message) : base(message)
    {
    }

    public FirebaseAuthException(string message, Exception innerException) : base(message, innerException)
    {
    }
}