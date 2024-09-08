using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using UseCases;
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
            
        services.AddSingleton<IUpdateHandler, BotClientUpdateHandler>();
        services.AddHostedService<TelegramBotBackgroundService>();

        return services;
    }
}