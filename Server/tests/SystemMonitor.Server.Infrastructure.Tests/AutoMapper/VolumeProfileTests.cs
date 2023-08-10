namespace SystemMonitor.Server.Infrastructure.Tests.AutoMapper;

using global::AutoMapper;
using SystemMonitor.Server.Infrastructure.AutoMapper.Profiles;
using Target = SystemMonitor.Server.Application.Monitor.Models.Volume;
using Source = SystemMonitor.Shared.Volumes.Volume;
using TargetDisk = SystemMonitor.Server.Application.Monitor.Models.VolumeDisk;
using SourceDisk = SystemMonitor.Shared.Volumes.VolumeDisk;

[TestClass]
public sealed class VolumeProfileTests
{
    private static readonly Source EXPECTED_SOURCE = new()
    {
        Disks = new List<SourceDisk>
        {
            new()
            {
                Length = 122,
                Number = 3,
                StartingOffset = 322,
            },
        },
        DisksCount = 2,
        Error = "error target",
        Name = "target name",
    };

    private static readonly Target EXPECTED_TARGET = new()
    {
        Disks = new List<TargetDisk>
        {
            new()
            {
                Length = 1,
                Number = 2,
                StartingOffset = 3,
            },
        },
        DisksCount = 1,
        Error = "error",
        Name = "source name",
    };

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
        },
        DisksCount = 1,
        Error = "error",
        Name = "source name",
    };

    private static readonly Target TARGET = new()
    {
        Disks = new List<TargetDisk>
        {
            new()
            {
                Length = 122,
                Number = 3,
                StartingOffset = 322,
            },
        },
        DisksCount = 2,
        Error = "error target",
        Name = "target name",
    };

    private readonly IMapper mapper;

    public VolumeProfileTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile<VolumeProfile>();
            config.AddProfile<VolumeDiskProfile>();
        });

        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Should_Map_Source_To_Target()
    {
        //GIVEN

        //WHEN
        var result = this.mapper.Map<Target>(SOURCE);

        //THEN
        result.Should()
            .BeEquivalentTo(EXPECTED_TARGET)
            ;
    }

    [TestMethod]
    public void Should_Map_Target_To_Source()
    {
        //GIVEN

        //WHEN
        var result = this.mapper.Map<Source>(TARGET);

        //THEN
        result.Should()
            .BeEquivalentTo(EXPECTED_SOURCE)
            ;
    }
}
