namespace SystemMonitor.Shared;

public abstract record Message
{
    public string ClientName { get; init; } = string.Empty;
    public string? Error { get; init; } = default;
    public abstract string MethodName { get; }
}
