using CleanArchitecture.ArchitectureTests.Extensions;
using CleanArchitecture.ArchitectureTests.Namespaces;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace CleanArchitecture.ArchitectureTests.Domain;
public class DomainLayerTests
{
    protected Assembly assembly;

    public DomainLayerTests()
    {
        assembly = Assembly.Load(ArchitectureNamespaces.DomainNamespace);
    }

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        string[] otherProjects =
        [
            ArchitectureNamespaces.ApplicationNamespace,
            ArchitectureNamespaces.PresentationAPINamespace
        ];

        //Act
        TestResult result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        string failingTypes = FailingTypesExtension.GetFailingTypesMessage(result);

        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    [Fact]
    public void DomainLayer_Entities_ShouldBeClasses()
    {
        //Arrange
        IEnumerable<Type> types = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Entities")
            .GetTypes();

        //Act
        List<Type> publicRecords = types
                 .Where(t => t.IsClass && t.IsPublic && t.GetMethods().Any(m => m.Name == "<Clone>$"))
                 .ToList();

        //Assert
        string message = "because there should be classes in the Entities namespace";
        List<string?> failingTypes = publicRecords.Select(t => t.FullName).ToList();

        if (failingTypes is { Count: > 0 })
            message = $"The following classes did not meet the architectural rules: {string.Join(", ", failingTypes)}";

        publicRecords
            .Should()
            .BeEmpty(message);
    }
}
