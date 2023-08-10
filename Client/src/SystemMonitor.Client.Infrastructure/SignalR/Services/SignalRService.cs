namespace SystemMonitor.Client.Infrastructure.SignalR.Services;

using Microsoft.AspNetCore.SignalR.Client;
using SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;

internal sealed class SignalRService : ISignalRService
{
    private readonly HubConnection connection;

    public SignalRService(HubConnection connection)
        => this.connection = connection;

    public async Task SendMessageAsync<T>(string methodName, T message, CancellationToken cancellationToken = default)
        => await this.connection.InvokeAsync(methodName, message, cancellationToken);
}
