namespace SystemMonitor.Client.Infrastructure.Common.Services;

using SystemMonitor.Client.Infrastructure.Common.Services.Interfaces;

internal sealed class ClientNameService : IClientNameService
{
    public string GetClientName()
        => Environment.MachineName;
}
