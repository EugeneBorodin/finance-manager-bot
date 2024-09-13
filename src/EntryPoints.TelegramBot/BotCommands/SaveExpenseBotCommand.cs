using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using UseCases.DTO;
using UseCases.Expenses.Commands;

namespace EntryPoints.TelegramBot.BotCommands;

public class SaveExpenseBotCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly ILogger<SaveExpenseBotCommand> _logger;
    
    public SaveExpenseBotCommand(IMediator mediator, ILogger<SaveExpenseBotCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<string> Execute(Message message)
    {
        try
        {
            var expenseRecordParts = message.Text.Split(' ');
            var expenseDto = new ExpenseDto
            {
                Category = expenseRecordParts[0].Substring(1),
                Value = Convert.ToDecimal(expenseRecordParts[1].Replace('.', ',')),
                MessageId = message.MessageId,
                ChannelId = message.Chat.Id,
                DateTime = new DateTimeOffset(message.Date)
            };
            
            var id = await _mediator.Send(new SaveExpenseCommand { ExpenseDto = expenseDto });
            return $"Расход сохранен в базу данных. Записи присвоен идентификатор: {id}.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}