namespace Jobs.Infrastructure;

public interface IRecurringJobScheduler
{
    public void AddOrUpdate<TParam>(string jobId, TParam param, string cronType) where TParam : JobParam;
    public void RemoveIfExists(string jobId);
    public string TriggerJob(string jobId);
}