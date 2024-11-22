using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Customers.Dtos;
using Domain.Entities;
using Domain.FluentResults;
using FluentResults;

namespace Application.Features.Customers.Queries;

public sealed class GetCustomerByIdQueryHandler : IQueryhandler<GetCustomerByIdQuery, CustomerDto>
{
    #region Private Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion Private Fields

    #region Public Constructors

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {  
        Customer? customerFound = await _unitOfWork.Customers.GetByIdAsync(request.CustormerId, cancellationToken);

        if (customerFound is null)
            return Result.Fail(NotFoundError.NotFound($"Customer not found by Id: {request.CustormerId}"));

        CustomerDto customer = new (customerFound.Id, customerFound.Name, customerFound.Email, customerFound.BirthDate);

        return Result.Ok(customer);
    }

    #endregion Public Methods
}