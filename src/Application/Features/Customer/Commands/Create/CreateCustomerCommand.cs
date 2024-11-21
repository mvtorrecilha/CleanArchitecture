using Application.Abstractions.Messaging;

namespace Application.Features.Customer.Commands.Create;

public sealed record CreateCustomerCommand(
    string Name,
    string Email,
    DateOnly BirthDate) : ICommand;
