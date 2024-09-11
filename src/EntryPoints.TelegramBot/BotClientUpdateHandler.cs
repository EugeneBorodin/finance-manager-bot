using System.Text.RegularExpressions;
using AutoMapper;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UseCases.DTO;
using UseCases.Expenses.Commands;
using DateTimeOffset = System.DateTimeOffset;

namespace EntryPoints.TelegramBot;

public class BotClientUpdateHandler : IUpdateHandler
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly string[] _commands = new [] { "/сводка" };
    
    public BotClientUpdateHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var (message, savedText) = update.Type switch
        {
            UpdateType.ChannelPost => (update.ChannelPost, "сохранено"),
            UpdateType.EditedChannelPost => (update.EditedChannelPost, "сохранено снова"),
            _ => (null, null)
        };
        
        if (message != null)
        {
            if (IsExpenseRecord(message.Text))
            {
                await SaveExpense(message);
            
                await botClient.EditMessageTextAsync(message.Chat.Id,
                    message.MessageId,
                    message.Text + " - " + savedText,
                    cancellationToken: cancellationToken);
            }
            else
            {
                string responseText = "К сожалению твой текст не распознан ни как комманда, ни как запись о расходах. Попробуй еще раз";
                
                if (IsCommand(message.Text))
                {
                    responseText = await ProcessMessage(message);
                }
                
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    responseText,
                    cancellationToken: cancellationToken);
            }
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
    
    private async Task SaveExpense(Message message)
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
            await _mediator.Send(new SaveExpenseCommand { ExpenseDto = expenseDto });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<string> ProcessMessage(Message message)
    {
        var command = new CalculateSummaryCommand
        {
            StartDate = new DateTimeOffset(new DateTime(2024, 09, 01).ToUniversalTime()),
            EndDate = new DateTimeOffset(new DateTime(2024, 09, 30).ToUniversalTime()),
            AccountBalance = 315_000,
            ChannelId = message.Chat.Id
        };
        
        return await _mediator.Send(command);
    }

    private bool IsCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return false;
        command = command.Trim().ToLower();
        return _commands.Contains(command);
    }
    
    private bool IsExpenseRecord(string expenseRecord)
    {
        if (string.IsNullOrWhiteSpace(expenseRecord)) return false;
        expenseRecord = expenseRecord.Trim().ToLower();
        return Regex.IsMatch(expenseRecord, @"^#[\w]+[' ']{1}[\d]+([.,][\d]+)?$");
    }
}