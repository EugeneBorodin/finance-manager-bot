using Jobs.Infrastructure;
using Jobs.Infrastructure.Reccuring;
using Jobs.Reccuring;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        return services.AddTransient<IJob<DailyCalculationJobParam>, DailyCalculationJob>();
    }
}