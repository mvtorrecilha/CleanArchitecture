namespace Application.Features.Customers.Dtos;

public record CustomerDto(Guid Id, string Name, string Email, DateTime BirthDate);
