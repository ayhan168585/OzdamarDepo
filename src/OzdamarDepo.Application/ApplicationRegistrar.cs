using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OzdamarDepo.Application.Behaviors;

namespace OzdamarDepo.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationRegistrar).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);
        
        return services;
    }
} 