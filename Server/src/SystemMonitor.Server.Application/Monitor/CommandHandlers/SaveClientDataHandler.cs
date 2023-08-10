namespace SystemMonitor.Server.Application.Monitor.CommandHandlers;

using SystemMonitor.Server.Application.Monitor.Commands;
using SystemMonitor.Server.Application.Monitor.Interfaces;

internal sealed class SaveClientDataHandler : IRequestHandler<SaveClientData>
{
    private readonly ILogger<SaveClientDataHandler> logger;
    private readonly IMonitorRepository repository;

    public SaveClientDataHandler(ILogger<SaveClientDataHandler> logger, IMonitorRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(SaveClientData request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to save client data with name '{ClientName}'", request.ClientData.ClientName);

        this.repository.SaveData(request.ClientData);

        await Task.CompletedTask;
    }
}
