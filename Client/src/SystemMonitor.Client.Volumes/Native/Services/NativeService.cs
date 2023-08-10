namespace SystemMonitor.Client.Volumes.Native.Services;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using SystemMonitor.Client.Volumes.Exceptions;
using SystemMonitor.Client.Volumes.Native.Models;
using SystemMonitor.Client.Volumes.Native.Services.Interfaces;

[ExcludeFromCodeCoverage]
internal sealed class NativeService : INativeService
{
    public (int disksCount, IEnumerable<DiskExtent>) GetVolumeDiskExtents(string volumeName)
    {
        var result = new List<DiskExtent>();

        using var volumeHandle = NativeMethods.CreateFile(volumeName, fileAccess: 0, NativeConstants.CREATE_FILE_SHARE_READ, IntPtr.Zero, NativeConstants.CREATE_FILE_CREATION_DISPOSITION_OPEN_EXISTING, flagsAndAttributes: 0, IntPtr.Zero);

        if (volumeHandle.IsInvalid)
        {
            var errorCode = Marshal.GetLastSystemError();
            var win32Exception = new Win32Exception(errorCode);

            throw new CreateFileException(win32Exception);
        }

        var volumeDiskExtents = new VolumeDiskExtents();
        var outBufferSize = (uint)Marshal.SizeOf(volumeDiskExtents);
        var outBuffer = Marshal.AllocHGlobal((int)outBufferSize);
        uint bytesReturned = 0;

        //TODO: read more data when volume extends multiple disks
        if (NativeMethods.DeviceIoControl(
                volumeHandle,
                NativeConstants.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS,
                IntPtr.Zero,
                inBufferSize: 0,
                outBuffer,
                outBufferSize,
                out bytesReturned,
                IntPtr.Zero) is false)
        {
            var errorCode = Marshal.GetLastSystemError();
            var win32Exception = new Win32Exception(errorCode);

            throw new ReadVolumeDiskExtentsException(win32Exception);
        }

        Marshal.PtrToStructure(outBuffer, volumeDiskExtents);

        result.Add(volumeDiskExtents.Extents);

        Marshal.FreeHGlobal(outBuffer);

        return (volumeDiskExtents.DisksCount, result);
    }

    public IEnumerable<string> GetVolumeNames()
    {
        const uint bufferLength = 1024;

        var volume = new StringBuilder((int)bufferLength, (int)bufferLength);
        var result = new List<string>();

        using var volumeHandle = NativeMethods.FindFirstVolume(volume, bufferLength);

        if (volumeHandle.IsInvalid)
        {
            var errorCode = Marshal.GetLastSystemError();
            var win32Exception = new Win32Exception(errorCode);

            throw new FindVolumeException(win32Exception);
        }

        do
        {
            var volumeName = volume.ToString().TrimEnd('\\');

            result.Add(volumeName);
        } while (NativeMethods.FindNextVolume(volumeHandle, volume, bufferLength));

        return result;
    }
}
