using System.Globalization;
using MediatR;
using Telegram.Bot.Types;
using UseCases.Expenses.Commands;

namespace EntryPoints.TelegramBot.BotCommands;

public class CalculateSummaryBotCommand : IBotCommand
{
    private readonly IMediator _mediator;
    public CalculateSummaryBotCommand(IMediator mediator)
    {
        _mediator = mediator;
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
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}