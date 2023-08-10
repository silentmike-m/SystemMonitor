namespace SystemMonitor.Client.Volumes.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public sealed class ReadVolumeDiskExtentsException : Exception
{
    public ReadVolumeDiskExtentsException(Exception? innerException = null)
        : base("Error on reading volume disk extents", innerException)
    {
    }

    public ReadVolumeDiskExtentsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
