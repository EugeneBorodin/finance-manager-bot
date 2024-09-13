using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace EntryPoints.TelegramBot.BotCommands;

public class HandleErrorBotCommand : IBotCommand
{
    private readonly ILogger<HandleErrorBotCommand> _logger;

    public HandleErrorBotCommand(ILogger<HandleErrorBotCommand> logger)
    {
        _logger = logger;
    }
    
    public Task<string> Execute(Message message)
    {
        _logger.LogInformation("Текст не распознан как команда для бота: {text}", message.Text);
        
        string responseText = "К сожалению твой текст не распознан ни как комманда, ни как запись о расходах. Попробуй еще раз";
        return Task.FromResult(responseText);
    }
}