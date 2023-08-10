namespace SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;

internal interface ISignalRService
{
    Task SendMessageAsync<T>(string methodName, T message, CancellationToken cancellationToken = default);
}
