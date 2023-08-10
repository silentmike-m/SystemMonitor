namespace SystemMonitor.Server.Infrastructure.SignalR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddMonitorSignalR(this IServiceCollection services)
    {
        services.AddSignalR();

        return services;
    }

    public static void UseSignalR(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<MonitorHub>(MonitorHub.PATTERN);
    }
}
