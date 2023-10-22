using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.Services;

public class ValidationService : IValidationService
{
    private readonly OperationResultBuilder<OperationResult> _operationResultBuilder;

    public ValidationService(OperationResultBuilder<OperationResult> operationResultBuilder)
    {
        _operationResultBuilder =
            operationResultBuilder ?? throw new ArgumentNullException(nameof(operationResultBuilder));
    }

    public OperationResult CheckAllValidation<T>(IEnumerable<Validation<T>> validations, T data)
    {
        var failedValidations =
            validations.Select(x => x.Validate(data)).Where(x => x.Status == ValidationStatus.Failed);
        return failedValidations.Any()
            ? _operationResultBuilder.SetFailureStatus().AddMessages(failedValidations.SelectMany(x => x.ErrorMessages))
                .Build()
            : _operationResultBuilder.SetSuccessStatus().Build();
    }
}