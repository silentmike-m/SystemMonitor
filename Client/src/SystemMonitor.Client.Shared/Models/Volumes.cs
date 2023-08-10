namespace SystemMonitor.Client.Shared.Models;

public sealed record Volumes
{
    public IReadOnlyList<Volume> List { get; init; } = new List<Volume>();
}
