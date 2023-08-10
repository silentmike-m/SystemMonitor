namespace SystemMonitor.Server.Infrastructure.Monitor.Services;

using Microsoft.Extensions.Options;
using SystemMonitor.Server.Application.Common;
using SystemMonitor.Server.Application.Monitor.Interfaces;
using SystemMonitor.Server.Application.Monitor.Models;

internal sealed class MonitorRepository : IMonitorRepository
{
    private readonly IFileService fileService;
    private readonly MonitorDataOptions options;

    public MonitorRepository(IFileService fileService, IOptions<MonitorDataOptions> options)
    {
        this.fileService = fileService;
        this.options = options.Value;
    }

    public void SaveData<T>(T clientData)
        where T : ClientData
    {
        var json = clientData.GetJson();

        var fileName = clientData.GetFileName();

        var path = Path.Combine(this.options.Directory, clientData.ClientName, clientData.DataName, fileName);

        this.fileService.WriteAllText(path, json);
    }
}
