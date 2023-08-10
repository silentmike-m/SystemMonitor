namespace SystemMonitor.Client.Infrastructure.Tests.Workers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemMonitor.Client.Infrastructure.Workers;
using SystemMonitor.Client.Shared.Commands;

[TestClass]
public sealed class WorkerTests
{
    [TestMethod]
    public async Task Should_Send_Get_Volumes_Information_Request()
    {
        //GIVEN
        var mediator = new Mock<ISender>();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IHostedService, Worker>()
            .AddSingleton(mediator.Object)
            .AddLogging()
            .BuildServiceProvider();

        var hostedService = serviceProvider.GetRequiredService<IHostedService>();

        //WHEN
        await hostedService.StartAsync(CancellationToken.None);

        //THEN
        mediator.Verify(service => service.Send(It.IsAny<GetVolumesInformation>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
