using Application.Abstractions.Data;
using Domain.Entities;
using FluentAssertions;
using global::Application.Features.Customers.Queries;
using Moq;

namespace CleanArchitecture.UnitTests.Application.Features.Customers.Queries;

public class GetCustomerByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetCustomerByIdQueryHandler _handler;

    public GetCustomerByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetCustomerByIdQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Email = "john.doe@example.com",
            BirthDate = new DateTime(1990, 1, 1),
            IsActive = true
        };

        _unitOfWorkMock
            .Setup(uow => uow.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var query = new GetCustomerByIdQuery(customerId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(customer.Id);
        result.Value.Name.Should().Be(customer.Name);
        result.Value.Email.Should().Be(customer.Email);
        result.Value.BirthDate.Should().Be(customer.BirthDate);

        _unitOfWorkMock.Verify(uow => uow.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _unitOfWorkMock
            .Setup(uow => uow.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        var query = new GetCustomerByIdQuery(customerId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        _unitOfWorkMock.Verify(uow => uow.Customers.GetByIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
    }
}

