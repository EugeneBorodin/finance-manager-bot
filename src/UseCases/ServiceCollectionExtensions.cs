using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UseCases.DTO;
using UseCases.Expenses.Commands;
using UseCases.Validators;

namespace UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddAutoMapper(assemblies);
        services.AddTransient<IValidator<ExpenseDto>, ExpenseValidator>();
        services.AddTransient<IValidator<CalculateSummaryCommand>, CalculateSummaryCommandValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        return services;
    }
}