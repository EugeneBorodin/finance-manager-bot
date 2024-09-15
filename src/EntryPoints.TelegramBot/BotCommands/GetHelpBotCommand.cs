using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace EntryPoints.TelegramBot.BotCommands;

public class GetHelpBotCommand : IBotCommand
{
    private readonly ILogger<GetHelpBotCommand> _logger;

    public GetHelpBotCommand(ILogger<GetHelpBotCommand> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> Execute(Message message)
    {
        try
        {
            var helpFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates/BotCommandsHelp.txt");
            return await File.ReadAllTextAsync(helpFilePath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}