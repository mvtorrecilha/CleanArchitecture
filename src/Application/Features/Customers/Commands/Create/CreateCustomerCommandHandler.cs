using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Behaviors;
using Domain.Entities;
using FluentResults;

namespace Application.Features.Customers.Commands.Create;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    #region Private Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion Private Fields

    #region Public Constructors

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    #endregion Public Constructors

    #region Public Methods

    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        CreateCustomerCommandValidator createCustomerValidator = new(request);
        if (!createCustomerValidator.ValidationResult.IsValid)
            return Result.Fail(FluentValidationBehavior.Create(nameof(CreateCustomerCommand), createCustomerValidator.ValidationResult.Errors));

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            BirthDate = request.BirthDate,
            IsActive = true
        };

        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }

    #endregion Public Methods
}