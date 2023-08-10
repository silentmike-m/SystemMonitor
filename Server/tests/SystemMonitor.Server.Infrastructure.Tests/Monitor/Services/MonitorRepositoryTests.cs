namespace SystemMonitor.Server.Infrastructure.Tests.Monitor.Services;

using Microsoft.Extensions.Options;
using SystemMonitor.Server.Application.Common;
using SystemMonitor.Server.Application.Monitor.Models;
using SystemMonitor.Server.Infrastructure.Monitor;
using SystemMonitor.Server.Infrastructure.Monitor.Services;

[TestClass]
public sealed class MonitorRepositoryTests
{
    private const string DIRECTORY = "tests";

    private static readonly VolumesData VOLUMES_DATA = new()
    {
        ClientName = "desktop-name",
        Error = "desktop error",
        ReceivedDate = new DateTime(year: 2023, month: 08, day: 10, hour: 10, minute: 22, second: 00),
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

    private readonly Mock<IFileService> fileService = new();

    private readonly IOptions<MonitorDataOptions> options = Options.Create(new MonitorDataOptions
    {
        Directory = DIRECTORY,
    });

    [TestMethod]
    public void Should_Save_Client_Data()
    {
        //GIVEN
        string? saveJson = null;
        string? savePath = null;

        this.fileService
            .Setup(service => service.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((path, json) =>
            {
                saveJson = json;
                savePath = path;
            });

        var repository = new MonitorRepository(this.fileService.Object, this.options);

        //WHEN
        repository.SaveData(VOLUMES_DATA);

        //THEN
        this.fileService.Verify(service => service.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        var expectedJson = VOLUMES_DATA.GetJson();

        saveJson.Should()
            .Be(expectedJson)
            ;

        var expectedPath = $@"{DIRECTORY}\{VOLUMES_DATA.ClientName}\{VOLUMES_DATA.DataName}\{VOLUMES_DATA.GetFileName()}";

        savePath.Should()
            .Be(expectedPath)
            ;
    }
}
