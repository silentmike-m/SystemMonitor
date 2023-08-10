namespace SystemMonitor.Shared.Volumes;

public sealed record VolumesInformationMessage : Message
{
    public override string MethodName => "SaveVolumesInformation";
    public IReadOnlyList<Volume> Volumes { get; init; } = new List<Volume>();
}
