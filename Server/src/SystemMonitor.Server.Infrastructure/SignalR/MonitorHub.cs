namespace SystemMonitor.Server.Infrastructure.SignalR;

using global::AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SystemMonitor.Server.Application.Common;
using SystemMonitor.Server.Application.Monitor.Commands;
using SystemMonitor.Server.Application.Monitor.Models;
using SystemMonitor.Shared.Volumes;
using Volume = SystemMonitor.Server.Application.Monitor.Models.Volume;

public sealed class MonitorHub : Hub
{
    public static readonly string PATTERN = "/MonitorHub";

    private readonly IDateTimeService dateTimeService;
    private readonly ILogger<MonitorHub> logger;
    private readonly IMapper mapper;
    private readonly ISender mediator;

    public MonitorHub(IDateTimeService dateTimeService, ILogger<MonitorHub> logger, IMapper mapper, ISender mediator)
    {
        this.dateTimeService = dateTimeService;
        this.logger = logger;
        this.mapper = mapper;
        this.mediator = mediator;
    }

    public async Task SaveVolumesInformation(VolumesInformationMessage message)
    {
        this.logger.LogInformation("Received volumes information");

        var data = this.MapClientData(message);

        var request = new SaveClientData(data);

        await this.mediator.Send(request, CancellationToken.None);
    }

    private ClientData MapClientData(VolumesInformationMessage message)
    {
        var volumes = this.mapper.Map<IReadOnlyList<Volume>>(message.Volumes);

        var data = new VolumesData
        {
            ReceivedDate = this.dateTimeService.GetNow(),
            ClientName = message.ClientName,
            Error = message.Error,
            Volumes = volumes,
        };

        return data;
    }
}
