using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoints.TelegramBot.BotCommands;

public class BotCommandFactory : IBotCommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public BotCommandFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IBotCommand GetCommand(string commandPattern)
    {
        switch (commandPattern)
        {
            case BotCommandPatterns.CalculateSummary:
                return new CalculateSummaryBotCommand(_serviceProvider.GetRequiredService<IMediator>());
            case BotCommandPatterns.SaveExpense:
                return new SaveExpenseBotCommand(_serviceProvider.GetRequiredService<IMediator>());
            default:
                return new HandleErrorBotCommand();
        }
    }
}