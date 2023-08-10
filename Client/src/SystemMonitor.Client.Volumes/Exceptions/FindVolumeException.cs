namespace SystemMonitor.Client.Volumes.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public sealed class FindVolumeException : Exception
{
    public FindVolumeException(Exception? innerException = null)
        : base("Error on reading volume information", innerException)
    {
    }

    public FindVolumeException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
