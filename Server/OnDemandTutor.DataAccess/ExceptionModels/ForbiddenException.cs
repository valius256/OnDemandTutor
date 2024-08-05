

namespace OnDemandTutor.DataAccess.ExceptionModels
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
