namespace SystemMonitor.Client.Volumes.Native;

internal static class NativeConstants
{
    public const uint CREATE_FILE_CREATION_DISPOSITION_OPEN_EXISTING = 3;
    public const uint CREATE_FILE_SHARE_READ = 0x00000001;
    public const int IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x560000;
}
