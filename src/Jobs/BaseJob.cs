using System.Diagnostics;
using Jobs.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Jobs;

public abstract class BaseJob<TParam> : IJob<TParam> where TParam : JobParam
{
    private readonly ILogger<BaseJob<TParam>> _logger;
    
    public BaseJob(ILogger<BaseJob<TParam>> logger)
    {
        _logger = logger;
    }
    public abstract Task ExecuteJob(TParam param);

    public async Task Execute(TParam param)
    {
        try
        {
            var jobName = GetType().Name;

            _logger.LogInformation("Starting job {jobName}.", jobName);

            var stopwatch = Stopwatch.StartNew();

            await ExecuteJob(param);

            stopwatch.Stop();

            _logger.LogInformation("Job {jobName} finished. Completion time: {ms} ms.", jobName,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}