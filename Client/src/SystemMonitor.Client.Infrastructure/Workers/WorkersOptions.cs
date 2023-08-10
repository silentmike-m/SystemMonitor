namespace SystemMonitor.Client.Infrastructure.Workers;

internal sealed record WorkersOptions
{
    public static readonly string SECTION_NAME = "Workers";

    public int DelayTimeoutInMinutes { get; set; } = 60;
}
