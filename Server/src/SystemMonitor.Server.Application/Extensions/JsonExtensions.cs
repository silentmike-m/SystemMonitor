namespace SystemMonitor.Server.Application.Extensions;

using System.Text.Json;

internal static class JsonExtensions
{
    public static string ToJson<T>(this T source, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        return JsonSerializer.Serialize(source, options);
    }
}
