using FluentAssertions;
using global::Application.Features.Customers.Queries;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Features.Customers.Queries;

public class GetCustomerByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCustomerDto_WhenQueryIsValid()
    {
        // Arrange
        var query = new GetCustomerByIdQuery(Guid.NewGuid()); 
        var handler = new GetCustomerByIdQueryHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue("handling a valid query should return a successful result");
        result.Value.Should().NotBeNull("result should contain a valid CustomerDto");
        result.Value.Id.Should().NotBeEmpty("CustomerDto should have a valid Id");
        result.Value.Name.Should().Be("Name 1", "the name in the mock should be 'Name 1'");
        result.Value.Email.Should().Be("email@email.com", "the email in the mock should be 'email@email.com'");
        result.Value.BirthDate.Should().Be(DateOnly.MinValue, "the birth date in the mock should be 'DateOnly.MinValue'");
    }
}
