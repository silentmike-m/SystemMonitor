namespace SystemMonitor.Client.Volumes;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SystemMonitor.Client.Volumes.Native.Services;
using SystemMonitor.Client.Volumes.Native.Services.Interfaces;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddVolumes(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddSingleton<INativeService, NativeService>();

        return services;
    }
}
