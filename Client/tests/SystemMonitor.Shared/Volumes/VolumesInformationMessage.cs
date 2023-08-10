namespace SystemMonitor.Shared.Volumes;

public sealed record VolumesInformationMessage
{
    public static readonly string METHOD_NAME = "Save_Volumes_Information";

    public string? Error { get; init; } = default;
    public IReadOnlyList<Volume> Volumes { get; init; } = new List<Volume>();
}
