namespace SystemMonitor.Client.Volumes.Native.Models;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal sealed class DiskExtent
{
    public int DiskNumber { get; set; } = default;
    public long ExtentStartingOffset { get; set; } = default;
    public long Length { get; set; } = default;
}
