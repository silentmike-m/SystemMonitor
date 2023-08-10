namespace SystemMonitor.Server.Infrastructure.File;

using SystemMonitor.Server.Application.Common;
using File = System.IO.File;

internal sealed class FileService : IFileService
{
    public void WriteAllText(string path, string value)
    {
        var directoryPath = Path.GetDirectoryName(path)!;

        if (Directory.Exists(directoryPath) is false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(path, value);
    }
}
