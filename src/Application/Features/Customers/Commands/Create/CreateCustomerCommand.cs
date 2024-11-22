using Application.Abstractions.Messaging;

namespace Application.Features.Customers.Commands.Create;

public sealed record CreateCustomerCommand(
    string Name,
    string Email,
    DateTime BirthDate) : ICommand;
