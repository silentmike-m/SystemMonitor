using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using SystemMonitor.Client.Infrastructure;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddConfiguration(configuration));

builder.ConfigureLogging(options =>
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    options.AddSerilog();
});

builder
    .ConfigureServices((_, services) =>
    {
        services.AddInfrastructure(configuration);
    });

try
{
    var app = builder.Build();

    app.UseInfrastructure();

    await app.StartAsync();

    Console.ReadKey();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
