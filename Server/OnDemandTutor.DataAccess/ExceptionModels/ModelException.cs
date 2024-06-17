namespace OnDemandTutor.DataAccess.ExceptionModels
{
    public class ModelException : Exception
    {
        public string PropertyName { get; }
        public string ErrorCode { get; }

        public ModelException(string propertyName, string message, string errorCode = null) : base(message)
        {
            PropertyName = propertyName;
            ErrorCode = errorCode;
        }
    }
}
