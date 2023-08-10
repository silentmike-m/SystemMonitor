namespace SystemMonitor.Client.Infrastructure.Workers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SystemMonitor.Client.Shared.Commands;
using SystemMonitor.Client.Shared.Extensions;

internal sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly WorkersOptions options;
    private readonly IServiceProvider serviceProvider;

    public Worker(ILogger<Worker> logger, IOptions<WorkersOptions> options, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.options = options.Value;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.BeginPropertyScope("WorkerName", nameof(Worker));

        this.logger.LogInformation("Worker process starting");

        while (stoppingToken.IsCancellationRequested is false)
            try
            {
                using var scope = this.serviceProvider.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

                var request = new GetVolumesInformation();

                await mediator.Send(request, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogWarning("Worker process has been canceled");
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "{Message}", exception.Message);
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(this.options.DelayTimeoutInMinutes), stoppingToken);
            }

        this.logger.LogInformation("Worker process stopping");
    }
}
