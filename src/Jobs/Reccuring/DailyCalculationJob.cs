using Jobs.Infrastructure.Reccuring;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using UseCases.Expenses.Commands;

namespace Jobs.Reccuring;

public class DailyCalculationJob : BaseJob<DailyCalculationJobParam>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMediator _mediator;
    private readonly ILogger<DailyCalculationJob> _logger;

    public DailyCalculationJob(ITelegramBotClient botClient, IMediator mediator, ILogger<DailyCalculationJob> logger) : base(logger)
    {
        _botClient = botClient;
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task ExecuteJob(DailyCalculationJobParam param)
    {
        var command = new CalculateSummaryCommand
        {
            StartDate = param.StartDate,
            EndDate = param.EndDate,
            AccountBalance = param.AccountBalance,
            ChannelId = param.ChannelId
        };
        
        var responseText = await _mediator.Send(command);
        
        await _botClient.SendTextMessageAsync(param.ChannelId, responseText);
    }
}