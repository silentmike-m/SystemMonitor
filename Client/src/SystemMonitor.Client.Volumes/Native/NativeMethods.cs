namespace SystemMonitor.Client.Volumes.Native;

using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using SystemMonitor.Client.Volumes.Native.Models;

internal static class NativeMethods
{
    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern SafeFileHandle CreateFile(string fileName, uint fileAccess, uint fileShare, IntPtr securityAttributes, uint creationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr template);

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern bool DeviceIoControl(SafeFileHandle safeHandler, uint controlCode, IntPtr inBuffer, uint inBufferSize, IntPtr outBuffer, uint outBufferSize, out uint bytesReturned, IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern FindVolumeSafeHandle FindFirstVolume([Out] StringBuilder volumeName, uint bufferLength);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool FindNextVolume(FindVolumeSafeHandle safeHandler, [Out] StringBuilder volumeName, uint bufferLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FindVolumeClose(IntPtr handle);
}
