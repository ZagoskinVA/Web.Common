namespace WebUtilities.Model;

public enum OperationStatus
{
    Success,
    Failed
}

public class OperationResult
{
    public OperationStatus Status { get; set; }
    public List<string> Messages { get; set; } = new();
}