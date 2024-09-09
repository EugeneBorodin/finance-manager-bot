using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities.Enums;
using Domain.Entities.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Messages.DTO;

namespace UseCases.Messages.Commands;

public class SaveMessageCommandHandler : IRequestHandler<SaveMessageCommand, Guid>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly IValidator<MessageDto> _validator;
    
    public SaveMessageCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper, IValidator<MessageDto> validator)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<Guid> Handle(SaveMessageCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var validatorResult = await _validator.ValidateAsync(request.MessageDto, cancellationToken);
        if (!validatorResult.IsValid)
        {
            throw new ValidationException(validatorResult.Errors);
        }
        
        return await UpsertEntity(request.MessageDto);
    }

    private async Task<Guid> UpsertEntity(MessageDto messageDto)
    {
        Message messageEntity;
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IFinanceManagerDbContext>();
            messageEntity = await dbContext.Messages.FirstOrDefaultAsync(m => 
                m.MessageId == messageDto.MessageId
                && m.ChatId == messageDto.ChatId);

            if (messageEntity != null)
            {
                messageEntity.Text = messageDto.Text;
                messageEntity.DateTime = messageDto.DateTime;
                dbContext.Messages.Update(messageEntity);
            }
            else
            {
                messageEntity = _mapper.Map<Message>(messageDto);
                messageEntity.Status = Status.New;
                await dbContext.Messages.AddAsync(messageEntity);
            }
            
            await dbContext.SaveChangesAsync();
        }

        return messageEntity.Id;
    }
}