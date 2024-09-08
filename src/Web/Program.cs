using DataAccess.Postrges;
using EntryPoints.TelegramBot;
using UseCases;
using Utils.Settings;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.Get<Settings>();
builder.Services.AddDataBase(settings.DataBaseSettings);
builder.Services.AddUseCases();
builder.Services.AddTelegramBot(settings.BotSettings);
var app = builder.Build();

app.MapGet("/", () => "Приложение для хостинга телеграм бота");

app.Run();