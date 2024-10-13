using Hangfire;
using Jobs.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs.Hangfire;

public class RecurringJobScheduler : IRecurringJobScheduler
{
    private readonly IServiceProvider _serviceProvider;
    
    public RecurringJobScheduler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void AddOrUpdate<TParam>(string jobId, TParam param, string cronType) where TParam : JobParam
    {
        var job = _serviceProvider.GetRequiredService<IJob<TParam>>();
        RecurringJob.AddOrUpdate(jobId, () => job.Execute(param), cronType);
    }

    public void RemoveIfExists(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
    }

    public string TriggerJob(string jobId)
    {
        var backgroundJobId = RecurringJob.TriggerJob(jobId); 
        return backgroundJobId;
    }
}