using API.Models.Customer;
using Application.Features.Customer.Commands.Create;
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

    [HttpPost("create")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorBaseData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerInput createCustomerInput, CancellationToken cancellationToken)
    {
        CreateCustomerCommand createCustomerCommand = new(createCustomerInput.Name, createCustomerInput.Email, createCustomerInput.BirthDate);

        Result result = await _sender.Send(createCustomerCommand, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return FluentResult(result);
    }
}
