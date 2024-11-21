using Application.Abstractions.Messaging;
using Application.Features.Customer.Dtos;

namespace Application.Features.Customer.Queries;

public sealed record GetCustomerByIdQuery(Guid CustormerId) : IQuery<CustomerDto>;