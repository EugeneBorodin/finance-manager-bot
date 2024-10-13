using System.Text.RegularExpressions;
using EntryPoints.TelegramBot.BotCommands;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EntryPoints.TelegramBot;

public class BotClientUpdateHandler : IUpdateHandler
{
    private readonly HashSet<string> _commandPatterns =
    [
        BotCommandPatterns.CalculateSummary,
        BotCommandPatterns.SaveExpense,
        BotCommandPatterns.GetHelp,
        BotCommandPatterns.ScheduleDailySummaryCalculation,
    ];
    
    private readonly IBotCommandFactory _commandFactory;
    private readonly ILogger<BotClientUpdateHandler> _logger;
    
    public BotClientUpdateHandler(IBotCommandFactory commandFactory, ILogger<BotClientUpdateHandler> logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
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

        if (message == null)
        {
            _logger.LogWarning("Был получен UpdateEvent, который не был обработан. Update: {@update}", update);
            return;
        }
        
        try
        {
            var commandPattern = GetCommandPattern(message.Text);
            var botCommand = _commandFactory.GetCommand(commandPattern);
            var responseText = await botCommand.Execute(message);

            if (commandPattern == BotCommandPatterns.SaveExpense)
            {
                await botClient.EditMessageTextAsync(message.Chat.Id,
                    message.MessageId,
                    message.Text + " - " + savedText,
                    cancellationToken: cancellationToken);
                
                _logger.LogInformation("{responseText} ChatId: {chatId}", responseText, message.Chat.Id);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    responseText,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            
            await botClient.SendTextMessageAsync(message.Chat.Id,
                "Во время обработки запроса произошла ошибка. Проверь введенные данные и попробуй снова.",
                cancellationToken: cancellationToken);
            
            _logger.LogError(ex, ex.Message);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _logger.LogError(exception.Message, exception);
        return Task.FromException(exception);
    }
    
    private string? GetCommandPattern (string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return null;
        command = command.Trim().ToLower();
        return _commandPatterns.SingleOrDefault(pattern => Regex.IsMatch(command, pattern));
    }
}