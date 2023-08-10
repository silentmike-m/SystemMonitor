namespace SystemMonitor.Client.Infrastructure.SignalR.Services;

using Microsoft.AspNetCore.SignalR.Client;
using SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;
using SystemMonitor.Shared;

internal sealed class SignalRService : ISignalRService
{
    private readonly HubConnection connection;

    public SignalRService(HubConnection connection)
        => this.connection = connection;

    public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : Message
        => await this.connection.InvokeAsync(message.MethodName, message, cancellationToken);
}
