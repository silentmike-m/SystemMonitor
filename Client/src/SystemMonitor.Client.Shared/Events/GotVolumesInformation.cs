namespace SystemMonitor.Client.Shared.Events;

using SystemMonitor.Client.Shared.Models;

public sealed record GotVolumesInformation : INotification
{
    public string? Error { get; set; } = default;
    public IReadOnlyList<Volume> List { get; init; } = new List<Volume>();
}
