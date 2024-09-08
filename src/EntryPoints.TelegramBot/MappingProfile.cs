using AutoMapper;
using Telegram.Bot.Types;
using UseCases.Messages.DTO;

namespace EntryPoints.TelegramBot;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Message, MessageDto>()
            .ForMember(destination => destination.SenderId,
                opts => opts.MapFrom(source => source.From != null ? source.From.Id : 0))
            .ForMember(destination => destination.SenderUsername,
            opts => opts.MapFrom(source => source.From != null ? source.From.Username : ""))
            .ForMember(destination => destination.ChatId,
                opts => opts.MapFrom(source => source.Chat.Id))
            .ForMember(destination => destination.DateTime,
                opts => opts.MapFrom(source => source.Date));
    }
}