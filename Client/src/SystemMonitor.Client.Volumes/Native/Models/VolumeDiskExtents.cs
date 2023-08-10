namespace SystemMonitor.Client.Volumes.Native.Models;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal sealed class VolumeDiskExtents
{
    public int DisksCount { get; set; } = default;
    public DiskExtent Extents { get; set; } = new();
}
