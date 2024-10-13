using Jobs.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                return new CalculateSummaryBotCommand(_serviceProvider.GetRequiredService<IMediator>(),
                    _serviceProvider.GetRequiredService<ILogger<CalculateSummaryBotCommand>>());
            case BotCommandPatterns.SaveExpense:
                return new SaveExpenseBotCommand(_serviceProvider.GetRequiredService<IMediator>(),
                    _serviceProvider.GetRequiredService<ILogger<SaveExpenseBotCommand>>());
            case BotCommandPatterns.GetHelp:
                return new GetHelpBotCommand(_serviceProvider.GetRequiredService<ILogger<GetHelpBotCommand>>());
            case BotCommandPatterns.ScheduleDailySummaryCalculation:
                return new ScheduleDailySummaryCalculationBotCommand(
                    _serviceProvider.GetRequiredService<ILogger<ScheduleDailySummaryCalculationBotCommand>>(),
                    _serviceProvider.GetRequiredService<IRecurringJobScheduler>());
            default:
                return new HandleErrorBotCommand(_serviceProvider.GetRequiredService<ILogger<HandleErrorBotCommand>>());
        }
    }
}