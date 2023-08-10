namespace SystemMonitor.Client.Volumes.Native.Services.Interfaces;

using SystemMonitor.Client.Volumes.Native.Models;

internal interface INativeService
{
    (int disksCount, IEnumerable<DiskExtent>) GetVolumeDiskExtents(string volumeName);
    IEnumerable<string> GetVolumeNames();
}
