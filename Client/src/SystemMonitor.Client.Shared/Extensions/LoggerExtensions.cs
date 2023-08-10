namespace SystemMonitor.Client.Shared.Extensions;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class LoggerExtensions
{
    public static IDisposable BeginPropertyScope(this ILogger logger, string name, object value)
        => logger.BeginScope(new Dictionary<string, object>
        {
            { name, value },
        })!;
}
