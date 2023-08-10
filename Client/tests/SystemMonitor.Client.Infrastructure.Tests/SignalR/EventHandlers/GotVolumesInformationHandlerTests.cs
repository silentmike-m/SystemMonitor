namespace SystemMonitor.Client.Infrastructure.Tests.SignalR.EventHandlers;

using global::AutoMapper;
using SystemMonitor.Client.Infrastructure.AutoMapper.Profiles;
using SystemMonitor.Client.Infrastructure.SignalR.EventHandlers;
using SystemMonitor.Client.Infrastructure.SignalR.Services.Interfaces;
using SystemMonitor.Client.Shared.Events;
using SystemMonitor.Shared.Volumes;
using Source = SystemMonitor.Client.Shared.Models.Volume;
using SourceDisk = SystemMonitor.Client.Shared.Models.VolumeDisk;

[TestClass]
public sealed class GotVolumesInformationHandlerTests
{
    private static readonly Source SOURCE = new()
    {
        Disks = new List<SourceDisk>
        {
            new()
            {
                Length = 1,
                Number = 2,
                StartingOffset = 3,
            },
            new()
            {
                Length = 3,
                Number = 4,
                StartingOffset = 5,
            },
        },
        DisksCount = 1,
        Name = "source name",
    };

    private static readonly VolumesInformationMessage EXPECTED_MESSAGE = new()
    {
        Error = SOURCE.Error,
        Volumes = new List<Volume>
        {
            new()
            {
                Disks = new List<VolumeDisk>
                {
                    new()
                    {
                        Length = 1,
                        Number = 2,
                        StartingOffset = 3,
                    },
                    new()
                    {
                        Length = 3,
                        Number = 4,
                        StartingOffset = 5,
                    },
                },
                DisksCount = 1,
                Name = "source name",
            },
        },
    };

    private readonly NullLogger<GotVolumesInformationHandler> logger = new();
    private readonly IMapper mapper;

    private readonly Mock<ISignalRService> signalRService = new();

    public GotVolumesInformationHandlerTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile<VolumeProfile>();
            config.AddProfile<VolumeDiskProfile>();
        });

        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task Should_Send_Message()
    {
        //GIVEN
        VolumesInformationMessage? volumesInformationMessage = null;

        this.signalRService
            .Setup(service => service.SendMessageAsync(It.IsAny<VolumesInformationMessage>(), It.IsAny<CancellationToken>()))
            .Callback<VolumesInformationMessage, CancellationToken>((message, _) => volumesInformationMessage = message);

        var notification = new GotVolumesInformation
        {
            List = new List<Source>
            {
                SOURCE,
            },
        };

        var handler = new GotVolumesInformationHandler(this.logger, this.mapper, this.signalRService.Object);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        this.signalRService.Verify(service => service.SendMessageAsync(It.IsAny<VolumesInformationMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        volumesInformationMessage.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(EXPECTED_MESSAGE)
            ;
    }

    [TestMethod]
    public async Task Should_Send_Message_With_Error()
    {
        //GIVEN
        VolumesInformationMessage? volumesInformationMessage = null;

        this.signalRService
            .Setup(service => service.SendMessageAsync(It.IsAny<VolumesInformationMessage>(), It.IsAny<CancellationToken>()))
            .Callback<VolumesInformationMessage, CancellationToken>((message, _) => volumesInformationMessage = message);

        var notification = new GotVolumesInformation
        {
            List = new List<Source>
            {
                SOURCE,
            },
            Error = "error",
        };

        var handler = new GotVolumesInformationHandler(this.logger, this.mapper, this.signalRService.Object);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        this.signalRService.Verify(service => service.SendMessageAsync(It.IsAny<VolumesInformationMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedMessage = new VolumesInformationMessage
        {
            Error = notification.Error,
        };

        volumesInformationMessage.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedMessage)
            ;
    }
}
