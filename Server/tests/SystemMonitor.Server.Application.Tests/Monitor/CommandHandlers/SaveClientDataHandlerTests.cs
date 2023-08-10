namespace SystemMonitor.Server.Application.Tests.Monitor.CommandHandlers;

using SystemMonitor.Server.Application.Monitor.CommandHandlers;
using SystemMonitor.Server.Application.Monitor.Commands;
using SystemMonitor.Server.Application.Monitor.Interfaces;
using SystemMonitor.Server.Application.Monitor.Models;

[TestClass]
public sealed class SaveClientDataHandlerTests
{
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

    private readonly NullLogger<SaveClientDataHandler> logger = new();
    private readonly Mock<IMonitorRepository> repository = new();

    [TestMethod]
    public async Task Should_Save_Client_Data()
    {
        //GIVEN
        ClientData? clientDataToSave = null;

        this.repository
            .Setup(service => service.SaveData(It.IsAny<ClientData>()))
            .Callback<ClientData>(clientData => clientDataToSave = clientData);

        var request = new SaveClientData(VOLUMES_DATA);

        var handler = new SaveClientDataHandler(this.logger, this.repository.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.repository.Verify(service => service.SaveData(It.IsAny<ClientData>()), Times.Once());

        clientDataToSave.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(request.ClientData);
    }
}
