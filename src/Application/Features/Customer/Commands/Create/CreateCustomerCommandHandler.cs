using Application.Abstractions.Messaging;
using Domain.Behaviors;
using FluentResults;

namespace Application.Features.Customer.Commands.Create;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    #region Public Constructors

    public CreateCustomerCommandHandler()
    {
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        CreateCustomerCommandValidator createCustomerValidator = new(request);
        if (!createCustomerValidator.ValidationResult.IsValid)
            return Result.Fail(FluentValidationBehavior.Create(nameof(CreateCustomerCommand), createCustomerValidator.ValidationResult.Errors));
        
        //TODO: INSERT CUSTOMER TO DATABASE

        return Result.Ok();
    }

    #endregion Public Methods
}