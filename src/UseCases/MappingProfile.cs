using AutoMapper;
using Domain.Entities.Models;
using UseCases.Messages.DTO;

namespace UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MessageDto, Message>();
    }
}