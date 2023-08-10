namespace SystemMonitor.Server.Infrastructure.Monitor;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SystemMonitor.Server.Application.Monitor.Interfaces;
using SystemMonitor.Server.Infrastructure.Monitor.Services;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddMonitorData(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MonitorDataOptions>(configuration.GetSection(MonitorDataOptions.SECTION_NAME));

        services.AddSingleton<IMonitorRepository, MonitorRepository>();
    }
}
