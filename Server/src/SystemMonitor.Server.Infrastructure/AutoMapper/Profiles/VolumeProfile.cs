namespace SystemMonitor.Server.Infrastructure.AutoMapper.Profiles;

using global::AutoMapper;
using Target = SystemMonitor.Server.Application.Monitor.Models.Volume;
using Source = SystemMonitor.Shared.Volumes.Volume;

internal sealed class VolumeProfile : Profile
{
    public VolumeProfile()
    {
        this.CreateMap<Source, Target>()
            .ForMember(target => target.DisksCount, options => options.MapFrom(source => source.DisksCount))
            .ForMember(target => target.Error, options => options.MapFrom(source => source.Error))
            .ForMember(target => target.Name, options => options.MapFrom(source => source.Name))
            .ReverseMap();
    }
}
