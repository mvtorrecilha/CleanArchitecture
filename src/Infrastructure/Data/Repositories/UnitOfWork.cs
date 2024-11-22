using Application.Abstractions.Data;
using Application.Features.Customers.Interfaces;

namespace Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private CustomerRepository? _customerRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public Task RollbackAsync()
    {
        _context.ChangeTracker.Clear();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}