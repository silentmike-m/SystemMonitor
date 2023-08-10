namespace SystemMonitor.Client.Infrastructure.SignalR;

internal sealed record SignalROptions
{
    public static readonly string SECTION_NAME = "SignalR";

    public string Url { get; set; } = string.Empty;
}
