namespace OnDemandTutor.DataAccess.ExceptionModels;

public class ValidationErrorModel
{
    public ValidationErrorModel(string errorMessage, string propertyName = null, string errorCode = null)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
}