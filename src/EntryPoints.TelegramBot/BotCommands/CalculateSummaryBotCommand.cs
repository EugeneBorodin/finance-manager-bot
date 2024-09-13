using System.Globalization;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using UseCases.Expenses.Commands;

namespace EntryPoints.TelegramBot.BotCommands;

public class CalculateSummaryBotCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly ILogger<CalculateSummaryBotCommand> _logger;
    public CalculateSummaryBotCommand(IMediator mediator, ILogger<CalculateSummaryBotCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<string> Execute(Message message)
    {
        try
        {
            var commandParts = message.Text.Split(' ');
            var startDateTime = DateTime.Parse(commandParts[1], new CultureInfo("ru-RU")).ToUniversalTime();
            var endDateTime = DateTime.Parse(commandParts[2], new CultureInfo("ru-RU")).ToUniversalTime();
            var balance = Convert.ToDecimal(commandParts[3].Replace('.', ','));
            
            var command = new CalculateSummaryCommand
            {
                StartDate = new DateTimeOffset(startDateTime),
                EndDate = new DateTimeOffset(endDateTime),
                AccountBalance = balance,
                ChannelId = message.Chat.Id
            };
        
            return await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}