using Serilog;

namespace API.Configurations;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(WebApplicationBuilder builder)
    {
        _ = builder.Host.UseSerilog((context, configuration) =>
        {
            IHostEnvironment env = context.HostingEnvironment;
            configuration
               .ReadFrom
               .Configuration(context.Configuration);

            if (!env.IsDevelopment())
            {
                string? ConnStr = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");

                configuration
                .WriteTo
                   .ApplicationInsights(ConnStr, TelemetryConverter.Traces);
            }
        });
    }
}
