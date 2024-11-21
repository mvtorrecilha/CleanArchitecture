using NetArchTest.Rules;

namespace CleanArchitecture.ArchitectureTests.Extensions;

public static class FailingTypesExtension
{
    public static string GetFailingTypesMessage(TestResult result)
    {
        if (result.FailingTypes is { Count: > 0 })
        {
            List<string?> failingTypes = result.FailingTypes.Select(t => t.FullName).ToList();
            return $"The following classes did not meet the architectural rules: {string.Join(", ", failingTypes)}";
        }

        return string.Empty;
    }
}