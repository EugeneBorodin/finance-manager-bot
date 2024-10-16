namespace EntryPoints.TelegramBot.BotCommands;

public static class BotCommandPatterns
{
    public const string CalculateSummary =
        @"^\/сводка[' '][\d]{2}[.][\d]{2}[.][\d]{4}[' '][\d]{2}[.][\d]{2}[.][\d]{4}[' '][\d]+$";
    
    public const string ScheduleDailySummaryCalculation =
        @"^\/ежедневно[' '][\d]{2}[:][\d]{2}[' '][\d]{2}[.][\d]{2}[.][\d]{4}[' '][\d]{2}[.][\d]{2}[.][\d]{4}[' '][\d]+$";

    public const string SaveExpense = @"^#[\w]+[' ']{1}[\d]+([.,][\d]+)?$";

    public const string GetHelp = @"^\/справка$";
}