namespace SystemMonitor.Server.Infrastructure.Tests.SignalR;

using global::AutoMapper;
using SystemMonitor.Server.Application.Common;
using SystemMonitor.Server.Application.Monitor.Commands;
using SystemMonitor.Server.Application.Monitor.Models;
using SystemMonitor.Server.Infrastructure.AutoMapper.Profiles;
using SystemMonitor.Server.Infrastructure.SignalR;
using SystemMonitor.Shared.Volumes;
using Source = SystemMonitor.Shared.Volumes.Volume;
using SourceDisk = SystemMonitor.Shared.Volumes.VolumeDisk;
using Volume = SystemMonitor.Server.Application.Monitor.Models.Volume;
using VolumeDisk = SystemMonitor.Server.Application.Monitor.Models.VolumeDisk;

[TestClass]
public sealed class MonitorHubTests
{
    private static readonly DateTime NOW = new(year: 2023, month: 08, day: 10, hour: 10, minute: 22, second: 00);

    private static readonly VolumesData EXPECTED_VOLUMES_DATA = new()
    {
        ClientName = "desktop-name",
        Error = "desktop error",
        ReceivedDate = NOW,
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

    private static readonly VolumesInformationMessage MESSAGE = new()
    {
        ClientName = "desktop-name",
        Error = "desktop error",
        Volumes = new List<Source>
        {
            new()
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
            },
        },
    };

    private readonly Mock<IDateTimeService> dateTimeService = new();
    private readonly NullLogger<MonitorHub> logger = new();
    private readonly IMapper mapper;
    private readonly Mock<ISender> mediator = new();

    public MonitorHubTests()
    {
        this.dateTimeService
            .Setup(service => service.GetNow())
            .Returns(NOW);

        var config = new MapperConfiguration(config =>
        {
            config.AddProfile<VolumeProfile>();
            config.AddProfile<VolumeDiskProfile>();
        });

        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task Should_Send_Save_Client_Data_Request()
    {
        //GIVEN
        SaveClientData? saveClientData = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<SaveClientData>(), It.IsAny<CancellationToken>()))
            .Callback<SaveClientData, CancellationToken>((request, _) => saveClientData = request);

        var hub = new MonitorHub(this.dateTimeService.Object, this.logger, this.mapper, this.mediator.Object);

        //WHEN
        await hub.SaveVolumesInformation(MESSAGE);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SaveClientData>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SaveClientData(EXPECTED_VOLUMES_DATA);

        saveClientData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}
