namespace OnDemandTutor.DataAccess
{
    public class ApiErrorActionResult
    {
        public string? Title { get; set; }
        public int Status { get; set; }
        public List<ValidationErrorModel>? Errors { get; set; }
    }

    public class ValidationErrorModel
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }
        public string ErrorCode { get; }

        public ValidationErrorModel(
            string propertyName,
            string errorMessage,
            string errorCode)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public ValidationErrorModel(
            string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
