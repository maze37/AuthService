using System.Reflection;
using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(
                    typeof(ICommandHandler<,>),
                    typeof(ICommandHandler<>)
                ))
            .AsSelfWithInterfaces()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
