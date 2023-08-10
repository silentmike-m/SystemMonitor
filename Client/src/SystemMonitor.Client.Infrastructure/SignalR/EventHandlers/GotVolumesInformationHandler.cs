namespace SystemMonitor.Client.Infrastructure.SignalR.EventHandlers;

using global::AutoMapper;
using SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;
using SystemMonitor.Client.Shared.Events;
using SystemMonitor.Shared.Volumes;
using SourceVolume = SystemMonitor.Client.Shared.Models.Volume;
using SourceVolumeDisk = SystemMonitor.Client.Shared.Models.VolumeDisk;
using TargetVolume = SystemMonitor.Shared.Volumes.Volume;
using TargetVolumeDisk = SystemMonitor.Shared.Volumes.VolumeDisk;

internal sealed class GotVolumesInformationHandler : INotificationHandler<GotVolumesInformation>
{
    private readonly ILogger<GotVolumesInformationHandler> logger;
    private readonly IMapper mapper;
    private readonly ISignalRService signalRService;

    public GotVolumesInformationHandler(ILogger<GotVolumesInformationHandler> logger, IMapper mapper, ISignalRService signalRService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.signalRService = signalRService;
    }

    public async Task Handle(GotVolumesInformation notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Received volumes information");

        var message = this.MapMessage(notification);

        await this.signalRService.SendMessageAsync(message, cancellationToken);
    }

    private VolumesInformationMessage MapMessage(GotVolumesInformation notification)
    {
        var volumes = string.IsNullOrEmpty(notification.Error)
            ? this.mapper.Map<List<Volume>>(notification.List)
            : new List<Volume>();

        var message = new VolumesInformationMessage
        {
            Error = notification.Error,
            Volumes = volumes,
        };

        return message;
    }
}
