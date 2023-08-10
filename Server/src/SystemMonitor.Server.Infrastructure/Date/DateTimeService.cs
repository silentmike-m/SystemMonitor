namespace SystemMonitor.Server.Infrastructure.Date;

using SystemMonitor.Server.Application.Common;

internal sealed class DateTimeService : IDateTimeService
{
    public DateTime GetNow()
        => DateTime.UtcNow;
}
