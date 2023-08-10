namespace SystemMonitor.Client.Infrastructure.AutoMapper.Profiles;

using global::AutoMapper;
using Source = SystemMonitor.Client.Shared.Models.VolumeDisk;
using Target = SystemMonitor.Shared.Volumes.VolumeDisk;

internal sealed class VolumeDiskProfile : Profile
{
    public VolumeDiskProfile()
    {
        this.CreateMap<Source, Target>()
            .ForMember(target => target.Length, options => options.MapFrom(source => source.Length))
            .ForMember(target => target.Number, options => options.MapFrom(source => source.Number))
            .ForMember(target => target.StartingOffset, options => options.MapFrom(source => source.StartingOffset))
            .ReverseMap()
            ;
    }
}
