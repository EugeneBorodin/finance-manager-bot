using MediatR;

namespace UseCases.Expenses.Commands;

public class CalculateSummaryCommand : IRequest<string>
{
    public long ChannelId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public decimal AccountBalance { get; set; }
}