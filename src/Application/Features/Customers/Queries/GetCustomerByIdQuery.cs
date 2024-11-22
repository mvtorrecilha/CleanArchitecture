using Application.Abstractions.Messaging;
using Application.Features.Customers.Dtos;

namespace Application.Features.Customers.Queries;

public sealed record GetCustomerByIdQuery(Guid CustormerId) : IQuery<CustomerDto>;