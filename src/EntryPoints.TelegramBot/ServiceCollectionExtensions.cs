using EntryPoints.TelegramBot.BotCommands;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Utils.Settings;

namespace EntryPoints.TelegramBot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, BotSettings botSettings)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services
            .AddHttpClient("TelegramBotClient")
            .AddTypedClient<ITelegramBotClient>((client, provider) =>
                new TelegramBotClient(botSettings.ApiToken, client));
        
        services.AddSingleton<IBotCommandFactory, BotCommandFactory>();
        services.AddSingleton<IBotCommand, HandleErrorBotCommand>();
        services.AddSingleton<IBotCommand, CalculateSummaryBotCommand>();
        services.AddSingleton<IUpdateHandler, BotClientUpdateHandler>();
        services.AddHostedService<TelegramBotBackgroundService>();

        return services;
    }
}