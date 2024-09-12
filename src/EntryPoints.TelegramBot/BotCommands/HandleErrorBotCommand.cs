using Telegram.Bot.Types;

namespace EntryPoints.TelegramBot.BotCommands;

public class HandleErrorBotCommand : IBotCommand
{
    public Task<string> Execute(Message message)
    {
        string responseText = "К сожалению твой текст не распознан ни как комманда, ни как запись о расходах. Попробуй еще раз";
        return Task.FromResult(responseText);
    }
}