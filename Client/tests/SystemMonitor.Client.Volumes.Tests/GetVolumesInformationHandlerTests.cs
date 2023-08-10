namespace SystemMonitor.Client.Volumes.Tests;

using System.ComponentModel;
using SystemMonitor.Client.Shared.Commands;
using SystemMonitor.Client.Shared.Events;
using SystemMonitor.Client.Shared.Models;
using SystemMonitor.Client.Volumes.CommandHandlers;
using SystemMonitor.Client.Volumes.Exceptions;
using SystemMonitor.Client.Volumes.Native.Models;
using SystemMonitor.Client.Volumes.Native.Services.Interfaces;

[TestClass]
public sealed class GetVolumesInformationHandlerTests
{
    private const string WIN32_EXCEPTION_MESSAGE = "Win32Exception fail";

    private readonly NullLogger<GetVolumesInformationHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();
    private readonly Mock<INativeService> nativeService = new();

    [TestMethod]
    public async Task Should_Publish_Notification()
    {
        //GIVEN
        GotVolumesInformation? gotVolumesInformation = null;

        const string volumeOneName = "volume one";
        const int volumeOneDisksCount = 1;
        const string volumeTwoName = "volume two";
        const int volumeTwoDisksCount = 2;

        var volumeOneDisks = new List<DiskExtent>
        {
            new()
            {
                DiskNumber = 0,
                ExtentStartingOffset = 22,
                Length = 1,
            },
        };

        var volumeTwoDisks = new List<DiskExtent>
        {
            new()
            {
                DiskNumber = 0,
                ExtentStartingOffset = 222,
                Length = 131,
            },
            new()
            {
                DiskNumber = 1,
                ExtentStartingOffset = 22,
                Length = 0,
            },
        };

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()))
            .Callback<GotVolumesInformation, CancellationToken>((notification, _) => gotVolumesInformation = notification);

        this.nativeService
            .Setup(service => service.GetVolumeNames())
            .Returns(new List<string>
            {
                volumeOneName,
                volumeTwoName,
            });

        this.nativeService
            .Setup(service => service.GetVolumeDiskExtents(It.IsAny<string>()))
            .Returns((string volumeName) =>
            {
                if (volumeName == volumeOneName)
                {
                    return new ValueTuple<int, IEnumerable<DiskExtent>>(volumeOneDisksCount, volumeOneDisks);
                }
                else
                {
                    return new ValueTuple<int, IEnumerable<DiskExtent>>(volumeTwoDisksCount, volumeTwoDisks);
                }
            });

        var request = new GetVolumesInformation();

        var handler = new GetVolumesInformationHandler(this.logger, this.mediator.Object, this.nativeService.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GotVolumesInformation
        {
            List = new List<Volume>
            {
                new()
                {
                    Disks = new List<VolumeDisk>
                    {
                        new()
                        {
                            Length = volumeOneDisks[0].Length,
                            Number = volumeOneDisks[0].DiskNumber,
                            StartingOffset = volumeOneDisks[0].ExtentStartingOffset,
                        },
                    },
                    DisksCount = volumeOneDisksCount,
                    Name = volumeOneName,
                },
                new()
                {
                    Disks = new List<VolumeDisk>
                    {
                        new()
                        {
                            Length = volumeTwoDisks[0].Length,
                            Number = volumeTwoDisks[0].DiskNumber,
                            StartingOffset = volumeTwoDisks[0].ExtentStartingOffset,
                        },
                        new()
                        {
                            Length = volumeTwoDisks[1].Length,
                            Number = volumeTwoDisks[1].DiskNumber,
                            StartingOffset = volumeTwoDisks[1].ExtentStartingOffset,
                        },
                    },
                    DisksCount = volumeTwoDisksCount,
                    Name = volumeTwoName,
                },
            },
        };

        gotVolumesInformation.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Get_Volume_Disk_Fail()
    {
        //GIVEN
        GotVolumesInformation? gotVolumesInformation = null;

        var win32Exception = new Win32Exception(WIN32_EXCEPTION_MESSAGE);

        const string volumeNameWithoutErrorName = "volume one";
        const int volumeDiskCountWithoutError = 1;
        const string volumeNameWithError = "volume two";

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()))
            .Callback<GotVolumesInformation, CancellationToken>((notification, _) => gotVolumesInformation = notification);

        this.nativeService
            .Setup(service => service.GetVolumeNames())
            .Returns(new List<string>
            {
                volumeNameWithoutErrorName,
                volumeNameWithError,
            });

        this.nativeService
            .Setup(service => service.GetVolumeDiskExtents(It.IsAny<string>()))
            .Returns((string volumeName) =>
            {
                if (volumeName == volumeNameWithError)
                {
                    throw new CreateFileException(win32Exception);
                }

                return new ValueTuple<int, IEnumerable<DiskExtent>>(volumeDiskCountWithoutError, new List<DiskExtent>());
            });

        var request = new GetVolumesInformation();

        var handler = new GetVolumesInformationHandler(this.logger, this.mediator.Object, this.nativeService.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GotVolumesInformation
        {
            List = new List<Volume>
            {
                new()
                {
                    DisksCount = volumeDiskCountWithoutError,
                    Name = volumeNameWithoutErrorName,
                },
                new()
                {
                    Error = WIN32_EXCEPTION_MESSAGE,
                    Name = volumeNameWithError,
                },
            },
        };

        gotVolumesInformation.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Get_Volume_Names_Fail()
    {
        //GIVEN
        GotVolumesInformation? gotVolumesInformation = null;

        var win32Exception = new Win32Exception(WIN32_EXCEPTION_MESSAGE);
        var exception = new FindVolumeException(win32Exception);

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()))
            .Callback<GotVolumesInformation, CancellationToken>((notification, _) => gotVolumesInformation = notification);

        this.nativeService
            .Setup(service => service.GetVolumeNames())
            .Throws(exception);

        var request = new GetVolumesInformation();

        var handler = new GetVolumesInformationHandler(this.logger, this.mediator.Object, this.nativeService.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Publish(It.IsAny<GotVolumesInformation>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GotVolumesInformation
        {
            Error = WIN32_EXCEPTION_MESSAGE,
        };

        gotVolumesInformation.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }
}
