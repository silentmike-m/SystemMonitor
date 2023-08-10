namespace SystemMonitor.Server.Application.Monitor.Models;

public sealed record Volume
{
    [JsonPropertyName("disks")] public IReadOnlyList<VolumeDisk> Disks { get; set; } = new List<VolumeDisk>();
    [JsonPropertyName("disks_count")] public int DisksCount { get; set; } = default;
    [JsonPropertyName("error")] public string? Error { get; set; } = default;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
}
