using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Messages.DTO;
using UseCases.Validators;

namespace UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddAutoMapper(assemblies);
        services.AddTransient<IValidator<MessageDto>, MessageValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        return services;
    }
}