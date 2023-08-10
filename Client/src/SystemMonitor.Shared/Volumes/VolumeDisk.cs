namespace SystemMonitor.Shared.Volumes;

public sealed record VolumeDisk
{
    public long Length { get; init; } = default;
    public int Number { get; init; } = default;
    public long StartingOffset { get; init; } = default;
}
