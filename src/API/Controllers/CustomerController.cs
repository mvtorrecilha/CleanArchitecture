using API.Models.Customer;
using Application.Features.Customers.Commands.Create;
using Application.Features.Customers.Dtos;
using Application.Features.Customers.Queries;
using Asp.Versioning;
using Domain.Abstractions.ErrorData;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/customer")]
public class CustomerController(ISender sender) : BaseController()
{
    #region Private Fields
    private readonly ISender _sender = sender;

    #endregion Private Fields

    /// <summary>
    /// Creates a new customer based on the provided input data.
    /// </summary>
    /// <param name="createCustomerInput">The data required to create a new customer.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// Returns a 200 OK status if the customer was created successfully.
    /// Returns a 400 Bad Request status if the provided input data is invalid.
    /// Returns a 500 Internal Server Error status for unexpected errors.
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerInput createCustomerInput, CancellationToken cancellationToken)
    {
        CreateCustomerCommand createCustomerCommand = new(createCustomerInput.Name, createCustomerInput.Email, createCustomerInput.BirthDate);

        Result result = await _sender.Send(createCustomerCommand, cancellationToken);
        if (result.IsSuccess)
            return Ok(); 

        return FluentResult(result);
    }

    /// <summary>
    /// Retrieves a customer by its unique identifier (ID).
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// Returns a 200 OK status with the customer details if found.
    /// Returns a 404 Not Found status if the customer does not exist.
    /// Returns a 500 Internal Server Error status for unexpected errors.
    /// </returns>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        GetCustomerByIdQuery getCustomerByIdQuery = new(id);

        Result<CustomerDto> result = await _sender.Send(getCustomerByIdQuery, cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);    

        return FluentResult(result);
    }
}
