namespace SystemMonitor.Client.Infrastructure.SignalR;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemMonitor.Client.Infrastructure.SignalR.Services;
using SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        var signalRConfig = configuration.GetSection(SignalROptions.SECTION_NAME).Get<SignalROptions>();
        signalRConfig ??= new SignalROptions();

        var connection = new HubConnectionBuilder()
            .WithUrl(signalRConfig.Url)
            .WithAutomaticReconnect()
            .Build();

        services.AddSingleton(connection);

        services.AddSingleton<ISignalRService, SignalRService>();
    }

    public static void UseSignalR(this IHost _, IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        var logger = loggerFactory.CreateLogger("SignalR connection");

        try
        {
            var connection = serviceProvider.GetRequiredService<HubConnection>();

            connection.Closed += error =>
            {
                if (error is not null)
                {
                    throw error;
                }

                logger.LogCritical(error, "SignalR connection error");

                return Task.CompletedTask;
            };

            connection.StartAsync().Wait();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Message}", exception.Message);

            throw;
        }
    }
}
