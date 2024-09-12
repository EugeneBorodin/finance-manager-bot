using System.Text.RegularExpressions;
using EntryPoints.TelegramBot.BotCommands;
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
    ];
    
    private readonly IBotCommandFactory _commandFactory;
    
    public BotClientUpdateHandler(IBotCommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
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
            var commandPattern = GetCommandPattern(message.Text);
            var botCommand = _commandFactory.GetCommand(commandPattern);
            var responseText = await botCommand.Execute(message);

            if (botCommand is SaveExpenseBotCommand)
            {
                await botClient.EditMessageTextAsync(message.Chat.Id,
                    message.MessageId,
                    message.Text + " - " + savedText,
                    cancellationToken: cancellationToken);
            }
            else
            {
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
    
    private string? GetCommandPattern (string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return null;
        command = command.Trim().ToLower();
        return _commandPatterns.SingleOrDefault(pattern => Regex.IsMatch(command, pattern));
    }
}