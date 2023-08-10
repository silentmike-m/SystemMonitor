namespace SystemMonitor.Client.Infrastructure.Workers;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static IServiceCollection AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WorkersOptions>(configuration.GetSection(WorkersOptions.SECTION_NAME));

        services.AddHostedService<Worker>();

        return services;
    }
}
