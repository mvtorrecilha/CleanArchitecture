using Application;
using Infrastructure;

namespace API.Configurations;

public static class DIConfiguration
{
    /// <summary>
    /// Adding Dependency Injection configuration
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDIConfiguration(this IServiceCollection services) =>
        services
          .AddApplicationServices()
          .AddInfraDI();
}