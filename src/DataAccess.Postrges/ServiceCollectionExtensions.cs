using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utils.Settings;

namespace DataAccess.Postrges;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, DataBaseSettings settings)
    {
        services.AddDbContext<IFinanceManagerDbContext, FinanceManagerDbContext>(opts =>
        {
            opts.UseNpgsql(settings.ConnectionString);
        });
        
        return services;
    }
}