namespace SystemMonitor.Server.Application.Common;

public interface IFileService
{
    void WriteAllText(string path, string value);
}
