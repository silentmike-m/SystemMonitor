namespace SystemMonitor.Shared.Volumes;

public sealed record VolumesInformationMessage : Message
{
    public override string MethodName => "Save_Volumes_Information";
    public IReadOnlyList<Volume> Volumes { get; init; } = new List<Volume>();
}
