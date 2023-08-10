namespace SystemMonitor.Client.Volumes.CommandHandlers;

using SystemMonitor.Client.Shared.Commands;
using SystemMonitor.Client.Shared.Events;
using SystemMonitor.Client.Shared.Models;
using SystemMonitor.Client.Volumes.Native.Services.Interfaces;

internal sealed class GetVolumesInformationHandler : IRequestHandler<GetVolumesInformation>
{
    private readonly ILogger<GetVolumesInformationHandler> logger;
    private readonly IPublisher mediator;
    private readonly INativeService nativeService;

    public GetVolumesInformationHandler(ILogger<GetVolumesInformationHandler> logger, IPublisher mediator, INativeService nativeService)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.nativeService = nativeService;
    }

    public async Task Handle(GetVolumesInformation request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        this.logger.LogInformation("Try to get volumes information");

        var result = new List<Volume>();

        try
        {
            var volumeNames = this.nativeService.GetVolumeNames();

            foreach (var volumeName in volumeNames)
            {
                var volume = this.GetVolume(volumeName);

                result.Add(volume);
            }

            await this.PublishNotification(result, exception: null, cancellationToken);
        }
        catch (Exception exception)
        {
            await this.PublishNotification(result, exception, cancellationToken);
        }
    }

    private Volume GetVolume(string volumeName)
    {
        var result = new Volume
        {
            Name = volumeName,
        };

        try
        {
            var volumeDisks = new List<VolumeDisk>();

            var (disksCount, diskExtents) = this.nativeService.GetVolumeDiskExtents(volumeName);

            foreach (var diskExtent in diskExtents)
            {
                var volumeDisk = new VolumeDisk
                {
                    Length = diskExtent.Length,
                    Number = diskExtent.DiskNumber,
                    StartingOffset = diskExtent.ExtentStartingOffset,
                };

                volumeDisks.Add(volumeDisk);
            }

            result.Disks = volumeDisks;
            result.DisksCount = disksCount;
        }
        catch (Exception exception)
        {
            result.Error = exception.InnerException?.Message ?? exception.Message;
        }

        return result;
    }

    private async Task PublishNotification(IReadOnlyList<Volume> volumes, Exception? exception, CancellationToken cancellationToken)
    {
        var notification = new GotVolumesInformation
        {
            Error = exception?.InnerException?.Message ?? exception?.Message,
            List = volumes,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
