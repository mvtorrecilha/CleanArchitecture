using Application.Features.Customers.Interfaces;

namespace Application.Abstractions.Data;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync();
}