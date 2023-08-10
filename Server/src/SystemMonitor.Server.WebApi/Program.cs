using Serilog;
using SystemMonitor.Server.Application;
using SystemMonitor.Server.Infrastructure;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(builder.Configuration));

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    app.UseInfrastructureEndpoints();

    await app.RunAsync();

    return EXIT_SUCCESS;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");

    return EXIT_FAILURE;
}
finally
{
    Log.CloseAndFlush();
}
