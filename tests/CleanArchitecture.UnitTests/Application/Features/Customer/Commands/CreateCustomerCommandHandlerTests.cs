namespace CleanArchitecture.UnitTests.Application.Features.Customer.Commands;
using FluentAssertions;
using global::Application.Features.Customer.Commands.Create;
using Xunit;

public class CreateCustomerCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        CreateCustomerCommand invalidCommand = new(
            "",
            "invalid-email",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)));

        CreateCustomerCommandHandler handler = new ();

        // Act
        var result = await handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue("validation errors should result in failure");
        result.Errors.Should().NotBeEmpty("validation errors should be returned");
        result.Errors.First().Message.Should().Contain("CreateCustomerCommand", "error messages should include the command name");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidationPasses()
    {
        // Arrange
        CreateCustomerCommand validCommand = new(
            "John Doe",
            "johndoe@example.com",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)));

        CreateCustomerCommandHandler handler = new ();

        // Act
        var result = await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue("valid commands should result in success");
    }
}

