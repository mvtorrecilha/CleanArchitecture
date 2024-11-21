using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Customer.Commands.Create;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    #region Public Constructors

    public CreateCustomerCommandValidator(CreateCustomerCommand createCustomerCommand)
    {
        RuleFor(customer => customer.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");

        RuleFor(customer => customer.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.");

        RuleFor(customer => customer.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .Must(birthDate => birthDate < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Birth date must be in the past.");

        ValidationResult = Validate(createCustomerCommand);
    }

    #endregion Public Constructors

    #region Public Properties

    public ValidationResult ValidationResult { get; }

    #endregion Public Properties
}