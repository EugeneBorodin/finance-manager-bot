using Telegram.Bot.Types;

namespace EntryPoints.TelegramBot.BotCommands;

public interface IBotCommand
{
    Task<string> Execute(Message message);
}