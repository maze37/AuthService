using AuthService.Infrastructure;
using AuthService.Application;
using AuthService.Infrastructure.Health;

namespace AuthService.Api.Configuration;

public static class Inject
{
    public static IServiceCollection ConfigureApp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddInfrastructure(configuration)
            .AddApplication()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddControllers();

        services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("database");

        return services;
    }
}
