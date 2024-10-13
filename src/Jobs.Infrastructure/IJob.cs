namespace Jobs.Infrastructure;

public interface IJob<in TParam> where TParam : class
{
    Task Execute(TParam param);
}