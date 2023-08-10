namespace SystemMonitor.Client.Infrastructure.Extensions;

using SystemMonitor.Client.Shared.Extensions;

public static class LoggerExtensions
{
    private const string FILE_LOGGER_PROPERTY_NAME = "LogToFile";

    public static IDisposable BeginFileLoggerScope(this ILogger logger)
        => logger.BeginPropertyScope(FILE_LOGGER_PROPERTY_NAME, value: true);
}
