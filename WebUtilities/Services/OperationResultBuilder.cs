using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.Services;

public class OperationResultBuilder<T> : IOperationResultBuilder<T> where T : OperationResult, new()
{
    private readonly T operationResult = new();

    public IOperationResultBuilder<T> AddMessage(string message)
    {
        operationResult.Messages.Add(message);
        return this;
    }

    public IOperationResultBuilder<T> AddMessages(IEnumerable<string> messages)
    {
        operationResult.Messages.AddRange(messages);
        return this;
    }

    public T Build()
    {
        return operationResult;
    }

    public IOperationResultBuilder<T> SetFailureStatus()
    {
        operationResult.Status = OperationStatus.Failed;
        return this;
    }

    public IOperationResultBuilder<T> SetSuccessStatus()
    {
        operationResult.Status = OperationStatus.Success;
        return this;
    }
}