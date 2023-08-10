namespace SystemMonitor.Server.Application.Monitor.Models;

public sealed record VolumeDisk
{
    [JsonPropertyName("Length")] public long Length { get; init; } = default;
    [JsonPropertyName("number")] public int Number { get; init; } = default;
    [JsonPropertyName("starting_offset")] public long StartingOffset { get; init; } = default;
}
