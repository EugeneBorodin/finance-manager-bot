using DataAccess.Postrges;
using EntryPoints.TelegramBot;
using UseCases;
using Utils.Settings;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var settings = builder.Configuration.Get<Settings>();
    builder.Services.AddSerilog();
    builder.Services.AddDataBase(settings.DataBaseSettings);
    builder.Services.AddUseCases();
    builder.Services.AddTelegramBot(settings.BotSettings);
    var app = builder.Build();

    app.MapGet("/", () => "Приложение для хостинга телеграм бота");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение неожиданно завершило работу");
}
finally
{
    Log.CloseAndFlush();
}