using System.Globalization;
using Jobs.Infrastructure;
using Jobs.Infrastructure.Reccuring;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace EntryPoints.TelegramBot.BotCommands;

public class ScheduleDailySummaryCalculationBotCommand : IBotCommand
{
    private const string DailyCalculationJobId = "DailyCalculationJob";
    private readonly ILogger<ScheduleDailySummaryCalculationBotCommand> _logger;
    private readonly IRecurringJobScheduler _recurringJobScheduler;
    
    public ScheduleDailySummaryCalculationBotCommand(ILogger<ScheduleDailySummaryCalculationBotCommand> logger, IRecurringJobScheduler recurringJobScheduler)
    {
        _logger = logger;
        _recurringJobScheduler = recurringJobScheduler;
    }
    
    public Task<string> Execute(Message message)
    {
        try
        {
            var commandParts = message.Text.Split(' ');
            var time = commandParts[1];
            var timeParts = time.Split(':');
            var hour = Convert.ToInt32(timeParts[0]);
            var minute = Convert.ToInt32(timeParts[1]);
            var startDateTime = DateTime.Parse(commandParts[2], new CultureInfo("ru-RU")).ToUniversalTime();
            var endDateTime = DateTime.Parse(commandParts[3], new CultureInfo("ru-RU")).ToUniversalTime();
            var balance = Convert.ToDecimal(commandParts[4].Replace('.', ','));
            
            var cronExpression = GetCroneExpression(hour, minute);
            
            _recurringJobScheduler.AddOrUpdate(DailyCalculationJobId,
                new DailyCalculationJobParam
                {
                    StartDate = new DateTimeOffset(startDateTime),
                    EndDate = new DateTimeOffset(endDateTime),
                    AccountBalance = balance,
                    ChannelId = message.Chat.Id
                }, cronExpression);

            var responseText =
                $"Процесс запланирован. Теперь ежедневно в {time} будет отправляться сводка о расходах по категориям и остатке с указанными выше параметрами.";
            
            return Task.FromResult(responseText);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private string GetCroneExpression(int hour, int minute)
    {
        var date = DateTime.Today.AddHours(hour).AddMinutes(minute).ToUniversalTime();
        var cronExpression = $"{date.Minute.ToString()} {date.Hour.ToString()} * * *";
        return cronExpression;
    }
}