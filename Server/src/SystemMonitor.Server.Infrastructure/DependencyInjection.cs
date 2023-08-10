namespace SystemMonitor.Server.Infrastructure;

using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SystemMonitor.Server.Application.Common;
using SystemMonitor.Server.Infrastructure.AutoMapper;
using SystemMonitor.Server.Infrastructure.Date;
using SystemMonitor.Server.Infrastructure.File;
using SystemMonitor.Server.Infrastructure.Monitor;
using SystemMonitor.Server.Infrastructure.SignalR;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddSingleton<IDateTimeService, DateTimeService>();

        services.AddSingleton<IFileService, FileService>();

        services.AddAutoMapper();

        services.AddMonitorSignalR();

        services.AddMonitorData(configuration);

        return services;
    }

    public static void UseInfrastructureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.UseSignalR();
    }
}
