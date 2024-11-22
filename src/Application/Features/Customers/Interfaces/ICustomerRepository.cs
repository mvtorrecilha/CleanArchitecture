namespace Application.Features.Customers.Interfaces;

public interface ICustomerRepository
{
    Task<Domain.Entities.Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Domain.Entities.Customer customer, CancellationToken cancellationToken);
    Task<IEnumerable<Domain.Entities.Customer>> GetAllAsync(CancellationToken cancellationToken);
}