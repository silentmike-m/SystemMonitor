namespace SystemMonitor.Client.Volumes.Native.Models;

using Microsoft.Win32.SafeHandles;

internal sealed class FindVolumeSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private FindVolumeSafeHandle()
        : base(true)
    {
    }

    public FindVolumeSafeHandle(IntPtr preexistingHandle, bool ownsHandle)
        : base(ownsHandle)
    {
        this.SetHandle(preexistingHandle);
    }

    protected override bool ReleaseHandle()
        => NativeMethods.FindVolumeClose(this.handle);
}
