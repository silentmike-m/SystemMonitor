namespace SystemMonitor.Server.Application.Monitor.Models;

using SystemMonitor.Server.Application.Extensions;

public sealed record VolumesData : ClientData
{
    public override string DataName => "VolumesData";

    [JsonPropertyName("volumes")] public IReadOnlyList<Volume> Volumes { get; init; } = new List<Volume>();

    public override string GetJson()
        => this.ToJson();
}
