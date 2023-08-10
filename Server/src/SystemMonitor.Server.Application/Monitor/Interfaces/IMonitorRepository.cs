namespace SystemMonitor.Server.Application.Monitor.Interfaces;

using SystemMonitor.Server.Application.Monitor.Models;

public interface IMonitorRepository
{
    void SaveData<T>(T clientData)
        where T : ClientData;
}
