namespace SystemMonitor.Client.Shared.Models;

public sealed record Volume
{
    public IReadOnlyList<VolumeDisk> Disks { get; set; } = new List<VolumeDisk>();
    public int DisksCount { get; set; } = default;
    public string? Error { get; set; } = default;
    public string Name { get; init; } = string.Empty;
}
