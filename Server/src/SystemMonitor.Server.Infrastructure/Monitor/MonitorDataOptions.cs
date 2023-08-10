namespace SystemMonitor.Server.Infrastructure.Monitor;

internal sealed record MonitorDataOptions
{
    public static readonly string SECTION_NAME = "MonitorData";

    public string Directory { get; set; } = string.Empty;
}
