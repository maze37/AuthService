using AuthService.Api.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting AuthService");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, logger) => logger
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ServiceName", "AuthService"));

    builder.Services.ConfigureApp(builder.Configuration);

    var app = builder.Build();
    app.ConfigureExtensions();
    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "AuthService terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
