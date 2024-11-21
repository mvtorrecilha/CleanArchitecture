using Application.Abstractions.Messaging;
using CleanArchitecture.ArchitectureTests.Extensions;
using CleanArchitecture.ArchitectureTests.Namespaces;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace CleanArchitecture.ArchitectureTests.Application;

public class ApplicationLayerTests
{
    protected Assembly assembly;
    public ApplicationLayerTests()
    {
        assembly = Assembly.Load(ArchitectureNamespaces.ApplicationNamespace);
    }

    [Fact]
    public void AplicationLayer_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        string[] otherProjects =
        [
            ArchitectureNamespaces.PresentationAPINamespace
        ];

        TestResult result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAll(otherProjects)
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    #region CQRS Query
    [Fact]
    public void ApplicationLayer_Cqrs_QueriesEndWithQuery()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should().HaveNameEndingWith("Query")
            .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Cqrs_QueryImplementsIQuery()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .DoNotHaveName("IQuery")
            .Should().ImplementInterface(typeof(IQuery<>))
            .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Cqrs_SealedQuery()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should().BeSealed()
            .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Cqrs_RecordQuery()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Query")
            .Should().BeMutable()
            .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Cqrs_ContainsAllQueryHandlers()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That().HaveNameEndingWith("QueryHandler")
            .Should()
            .ResideInNamespace(ArchitectureNamespaces.ApplicationNamespace)
            .GetResult();

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Cqrs_QueryHandlerImplementsIQueryHandler()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .And()
            .DoNotHaveName("IQueryHandler")
            .And()
            .DoNotHaveNameStartingWith("Base")
            .Should()
            .ImplementInterface(typeof(IQueryhandler<,>))
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    #endregion CQRS Query

    #region CQRS Command

    [Fact]
    public void ApplicationLayer_Cqrs_CommandsEndWithCommand()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should().HaveNameEndingWith("Command")
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    [Fact]
    public void ApplicationLayer_Cqrs_CommandImplementsICommand()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .DoNotHaveName("IBaseCommand")
            .And()
            .DoNotHaveName("ICommand")
            .Should().ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    [Fact]
    public void ApplicationLayer_Cqrs_SealedCommands()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should().BeSealed()
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    [Fact]
    public void ApplicationLayer_Cqrs_RecordCommands()
    {
        //Arrange
        IEnumerable<Type> result = Types
            .InCurrentDomain()
            .That()
            .HaveNameEndingWith("Command")
            .GetTypes()
            .Where(type => type.IsClass);

        foreach (Type? type in result)
        {
            //Act
            bool isRecord = type
                .GetMethods()
                .Any(m => m.Name == "<Clone>$");

            //Assert
            isRecord
                .Should()
                .BeTrue($"class {type.FullName} should be a record");
        }
    }

    [Fact]
    public void ApplicationLayer_Cqrs_CommandHandlerImplementsICommandHandler()
    {
        //Arrange
        TestResult result = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .And()
            .DoNotHaveName("ICommandHandler")
            .And()
            .DoNotHaveNameStartingWith("Base")
            .Should()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<>))
            .GetResult();

        //Act
        string failingTypes = FailingTypesExtension
            .GetFailingTypesMessage(result);

        //Assert
        result.IsSuccessful
            .Should()
            .BeTrue(failingTypes);
    }

    #endregion CQRS Command

    [Fact]
    public void ApplicationLayer_DTOs_Record()
    {
        //Arrange
        IEnumerable<Type> result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Dto")
            .GetTypes()
            .Where(type => type.IsClass);

        foreach (Type? type in result)
        {
            //Act
            bool isRecord = type
                .GetMethods()
                .Any(m => m.Name == "<Clone>$");

            //Assert
            isRecord
                .Should()
                .BeTrue($"class {type.FullName} should be a record");
        }
    }
}