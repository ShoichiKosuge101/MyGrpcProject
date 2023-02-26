using MyGrpcProject.Services;
using NLog;
using NLog.Web;

var _logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
_logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Additional configuration is required to successfully run gRPC on macOS.
    // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddGrpc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<GreeterService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch(Exception ex)
{
    // NLog: catch setup errors
    _logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
