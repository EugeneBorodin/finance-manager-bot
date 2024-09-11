using AutoMapper;
using Domain.Entities.Models;
using UseCases.DTO;

namespace UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExpenseDto, Expense>().ForMember(destinationMember => destinationMember.DateTime, 
            opts => opts.MapFrom(sourceMember => sourceMember.DateTime.ToUniversalTime()));
    }
}