namespace WebUtilities.Model;

public enum ValidationStatus
{
    Success,
    Failed
}

public class ValidationResult
{
    public ValidationResult(ValidationStatus status) : this(status, Array.Empty<string>())
    {
    }

    public ValidationResult(ValidationStatus status, string errorMessage) : this(status, new[] {errorMessage})
    {
    }

    public ValidationResult(ValidationStatus status, IEnumerable<string> errorMessages)
    {
        Status = status;
        ErrorMessages = errorMessages;
    }

    public ValidationStatus Status { get; }
    public IEnumerable<string> ErrorMessages { get; }
}