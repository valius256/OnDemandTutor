namespace OnDemandTutor.API.Middlesware;

public class ApiErrorActionResult
{
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public List<ValidationErrorModel> Errors { get; set; } = new List<ValidationErrorModel>();
}

public class ValidationErrorModel
{
    public ValidationErrorModel(string errorMessage, string? propertyName = null, string? errorCode = null)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public string? PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
}