namespace SystemMonitor.Server.Application.Monitor.Models;

public abstract record ClientData
{
    [JsonPropertyName("client_name")] public string ClientName { get; init; } = string.Empty;
    [JsonPropertyName("data_name")] public abstract string DataName { get; }
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("received_date")] public DateTime ReceivedDate { get; init; } = DateTime.MinValue;

    public string GetFileName()
        => $"{this.ReceivedDate.ToString("yyyyMMddTHHmmss")}.json";

    public abstract string GetJson();
}
