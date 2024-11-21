using Asp.Versioning;

namespace API.Configurations;

/// <summary>
/// Api versioning configuration
/// </summary>
public static class ApiVersioningConfiguration
{
    /// <summary>
    /// Adding api versioning configuration
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static void AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
