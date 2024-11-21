namespace Application.Features.Customer.Dtos;

public record CustomerDto(Guid Id, string Name, string Email, DateOnly BirthDate);
