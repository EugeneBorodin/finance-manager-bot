namespace Jobs.Infrastructure;

public abstract class JobParam
{
    public long ChannelId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public decimal AccountBalance { get; set; }
}