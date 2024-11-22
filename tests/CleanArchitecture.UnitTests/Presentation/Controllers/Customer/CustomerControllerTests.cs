using API.Controllers;
using API.Models.Customer;
using Application.Features.Customers.Commands.Create;
using Application.Features.Customers.Dtos;
using Application.Features.Customers.Queries;
using Domain.FluentResults;
using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CleanArchitecture.UnitTests.Presentation.Controllers.Customer;

public class CustomerControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _controller = new CustomerController(_senderMock.Object);
    }

    #region CreateCustomerAsync Tests

    [Fact]
    public async Task CreateCustomerAsync_ShouldReturnOk_WhenCommandIsSuccessful()
    {
        // Arrange
        var customerInput = new CustomerInput { Name = "John Doe", Email = "john.doe@example.com", BirthDate = DateTime.Now };
        var createCustomerCommand = new CreateCustomerCommand(customerInput.Name, customerInput.Email, customerInput.BirthDate);

        _senderMock.Setup(sender => sender.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(Result.Ok());

        // Act
        var result = await _controller.CreateCustomerAsync(customerInput, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>(); // Expecting HTTP 200 OK
    }

    [Fact]
    public async Task CreateCustomerAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var customerInput = new CustomerInput { Name = "", Email = "invalid-email", BirthDate = DateTime.MinValue };
        var createCustomerCommand = new CreateCustomerCommand(customerInput.Name, customerInput.Email, customerInput.BirthDate);

        Result expectedResult = Result.Fail(BadRequestError.Conflict("invalid input"));
        _senderMock.Setup(sender => sender.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateCustomerAsync(customerInput, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task CreateCustomerAsync_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var customerInput = new CustomerInput { Name = "John Doe", Email = "john.doe@example.com", BirthDate = DateTime.Now };
        var createCustomerCommand = new CreateCustomerCommand(customerInput.Name, customerInput.Email, customerInput.BirthDate);

        Result expectedResult = Result.Fail(InternalServerError.UnexpectedError("Error processing request"));
        _senderMock.Setup(sender => sender.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateCustomerAsync(customerInput, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    #endregion CreateCustomerAsync Tests

    #region GetCustomerByIdAsync Tests

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnOk_WhenCustomerFound()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customerDto = new CustomerDto(customerId, "John Doe", "john.doe@example.com", DateOnly.FromDateTime(DateTime.Now));

        _senderMock.Setup(sender => sender.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(Result.Ok(customerDto));

        // Act
        var result = await _controller.GetCustomerByIdAsync(customerId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>(); // Expecting HTTP 200 OK with CustomerDto in the body
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(customerDto); // Ensure the returned CustomerDto is correct
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnNotFound_WhenCustomerNotFound()
    {
        // Arrange
        var customerId = Guid.NewGuid();


        Result expectedResult = Result.Fail(NotFoundError.NotFound("Erro ao processar a solicitação"));

        _senderMock.Setup(sender => sender.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetCustomerByIdAsync(customerId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        Result expectedResult = Result.Fail(InternalServerError.UnexpectedError("Error processing request"));

        _senderMock.Setup(sender => sender.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetCustomerByIdAsync(customerId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    #endregion GetCustomerByIdAsync Tests
}

