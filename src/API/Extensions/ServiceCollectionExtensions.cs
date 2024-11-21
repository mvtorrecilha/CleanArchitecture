using API.Configurations;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDIConfiguration();

        services.AddApiVersioningConfiguration();

        services.ConfigureCors();
        return services;
    }
}
