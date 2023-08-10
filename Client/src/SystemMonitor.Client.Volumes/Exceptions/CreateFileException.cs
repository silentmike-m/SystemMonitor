namespace SystemMonitor.Client.Volumes.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public sealed class CreateFileException : Exception
{
    public CreateFileException(Exception? innerException = null)
        : base("Error on creating file", innerException)
    {
    }

    public CreateFileException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
