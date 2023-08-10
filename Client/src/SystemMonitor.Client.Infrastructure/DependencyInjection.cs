namespace SystemMonitor.Client.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemMonitor.Client.Infrastructure.AutoMapper;
using SystemMonitor.Client.Infrastructure.Common.Services;
using SystemMonitor.Client.Infrastructure.Common.Services.Interfaces;
using SystemMonitor.Client.Infrastructure.SignalR;
using SystemMonitor.Client.Infrastructure.Workers;
using SystemMonitor.Client.Volumes;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddAutoMapper();

        services.AddWorkers(configuration);

        services.AddSignalR(configuration);

        services.AddVolumes();

        services.AddSingleton<IClientNameService, ClientNameService>();

        return services;
    }

    public static void UseInfrastructure(this IHost app)
    {
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        app.UseSignalR(serviceScope.ServiceProvider);
    }
}
