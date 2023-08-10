namespace SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;

using SystemMonitor.Shared;

internal interface ISignalRService
{
    Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : Message;
}
