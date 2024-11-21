using Domain.Abstractions;
using Domain.FluentResults;
using FluentResults;
using FluentValidation.Results;

namespace Domain.Behaviors;

public class FluentValidationBehavior
{
    public static Error Create(string entityName, List<ValidationFailure> failures)
    {
        string message = $"Foram encontradas inconsistências na entidade: {entityName}";

        ErrorBase result = BadRequestError.Conflict(message);

        foreach (ValidationFailure failure in failures)
        {
            result.Reasons.Add(BadRequestError.Conflict(failure.ErrorMessage));
        }

        return result;
    }
}
