using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UseCases.DTO;

namespace UseCases.Expenses.Commands;

public class SaveExpenseCommandHandler : IRequestHandler<SaveExpenseCommand, long>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly IValidator<ExpenseDto> _validator;
    
    public SaveExpenseCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper, IValidator<ExpenseDto> validator)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<long> Handle(SaveExpenseCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var validatorResult = await _validator.ValidateAsync(request.ExpenseDto, cancellationToken);
        if (!validatorResult.IsValid)
        {
            throw new ValidationException(validatorResult.Errors);
        }
        
        return await UpsertEntity(request.ExpenseDto);
    }

    private async Task<long> UpsertEntity(ExpenseDto expenseDto)
    {
        Expense expenseEntity;
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IFinanceManagerDbContext>();
            expenseEntity = await dbContext.Expenses.FirstOrDefaultAsync(e => 
                e.MessageId == expenseDto.MessageId
                && e.ChannelId == expenseDto.ChannelId);

            if (expenseEntity != null)
            {
                expenseEntity.Value = expenseDto.Value;
                dbContext.Expenses.Update(expenseEntity);
            }
            else
            {
                expenseEntity = _mapper.Map<Expense>(expenseDto);
                await dbContext.Expenses.AddAsync(expenseEntity);
            }
            
            await dbContext.SaveChangesAsync();
        }

        return expenseEntity.Id;
    }
}