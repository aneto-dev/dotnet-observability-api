using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace DotNetObservability.Api.Observability;

public static class LoggingExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.GrafanaLoki(
                    uri: "http://localhost:3100",
                    labels: new[]
                    {
                        new LokiLabel { Key = "app", Value = "dotnet-observability-api" },
                        new LokiLabel { Key = "environment", Value = "local" }
                    });
        });
    }
}
