using FluentResults;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQueryhandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
