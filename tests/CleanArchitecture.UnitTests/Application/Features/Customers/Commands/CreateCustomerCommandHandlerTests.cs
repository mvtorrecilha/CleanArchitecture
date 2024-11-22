using Application.Abstractions.Data;
using Domain.Entities;
using FluentAssertions;
using global::Application.Features.Customers.Commands.Create;
using Moq;

namespace CleanArchitecture.UnitTests.Application.Features.Customers.Commands;


public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateCustomerCommandHandler(_unitOfWorkMock.Object);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenCustomerIsSuccessfullyCreated()
    {
        // Arrange
        var command = new CreateCustomerCommand("John Doe", "john.doe@example.com", Convert.ToDateTime("1990-01-01"));
        _unitOfWorkMock.Setup(uow => uow.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWorkMock.Verify(uow => uow.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenValidationFails()
    {
        // Arrange
        var command = new CreateCustomerCommand("", "invalid-email", DateTime.MinValue);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        _unitOfWorkMock.Verify(uow => uow.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenUnitOfWorkFailsToCommit()
    {
        // Arrange
        var command = new CreateCustomerCommand("John Doe", "john.doe@example.com", DateTime.Now);
        _unitOfWorkMock.Setup(uow => uow.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Database commit failed"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database commit failed");
        _unitOfWorkMock.Verify(uow => uow.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion Handle Tests
}

