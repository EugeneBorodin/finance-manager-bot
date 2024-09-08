using AutoMapper;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UseCases.Messages.Commands;
using UseCases.Messages.DTO;

namespace EntryPoints.TelegramBot;

public class BotClientUpdateHandler : IUpdateHandler
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BotClientUpdateHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (update.Message != null)
        {
            await HandleMessage(update.Message);
        }
        else if (update.EditedMessage != null)
        {
            await HandleMessage(update.EditedMessage);
        }
        else if (update.ChannelPost != null)
        {
            await HandleMessage(update.ChannelPost);
        }
        else if (update.EditedChannelPost != null)
        {
            await HandleMessage(update.EditedChannelPost);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
    
    private async Task SaveMessage(Message message)
    {
        try
        {
            var messageDto = _mapper.Map<MessageDto>(message);
            await _mediator.Send(new SaveMessageCommand { MessageDto = messageDto });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task HandleMessage(Message message)
    {
        await SaveMessage(message);
    }
}