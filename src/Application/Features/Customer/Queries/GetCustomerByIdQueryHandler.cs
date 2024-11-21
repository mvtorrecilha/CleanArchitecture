using Application.Abstractions.Messaging;
using Application.Features.Customer.Dtos;
using FluentResults;

namespace Application.Features.Customer.Queries;

public sealed class GetCustomerByIdQueryHandler : IQueryhandler<GetCustomerByIdQuery, CustomerDto>
{
    #region Public Constructors

    public GetCustomerByIdQueryHandler()
    {
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: Retrieves a customer by its unique identifier (ID) from Database
        return Result.Ok(new CustomerDto(Guid.NewGuid(),"Name 1", "email@email.com", DateOnly.MinValue));
    }

    #endregion Public Methods
}