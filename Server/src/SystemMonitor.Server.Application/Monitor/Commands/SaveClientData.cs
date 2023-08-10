namespace SystemMonitor.Server.Application.Monitor.Commands;

using SystemMonitor.Server.Application.Monitor.Models;

public sealed record SaveClientData : IRequest
{
    public ClientData ClientData { get; private set; }

    public SaveClientData(ClientData clientData)
        => this.ClientData = clientData;
}
