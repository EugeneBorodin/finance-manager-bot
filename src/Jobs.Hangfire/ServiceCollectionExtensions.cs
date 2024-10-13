using Hangfire;
using Hangfire.PostgreSql;
using Jobs.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Utils.Settings;

namespace Jobs.Hangfire;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobScheduler(this IServiceCollection services, JobSchedulerSettings settings)
    {
        return services
            .AddSingleton<IRecurringJobScheduler, RecurringJobScheduler>()
            .AddHangfire(cfg => cfg.UsePostgreSqlStorage(opts => 
            opts.UseNpgsqlConnection(settings.ConnectionString)));
    }
}