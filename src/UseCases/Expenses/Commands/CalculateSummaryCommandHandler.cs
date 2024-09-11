using System.Text;
using AutoMapper;
using DataAccess.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UseCases.Expenses.Commands;

public class CalculateSummaryCommandHandler : IRequestHandler<CalculateSummaryCommand, string>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly IValidator<CalculateSummaryCommand> _validator;
    
    public CalculateSummaryCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper, IValidator<CalculateSummaryCommand> validator)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<string> Handle(CalculateSummaryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IFinanceManagerDbContext>();
            var aggregation = await dbContext.Expenses
                .Where(e => e.ChannelId == request.ChannelId &&
                            e.DateTime >= request.StartDate &&
                            e.DateTime <= request.EndDate)
                .GroupBy(e => e.Category)
                .Select(group => new { Category = group.Key, Sum = group.Sum(e => e.Value) })
                .ToListAsync(cancellationToken: cancellationToken);
            
            var sb = new StringBuilder();
            decimal spent = 0;
            sb.Append($"Сводка за период {request.StartDate.ToLocalTime():dd.MM.yyyy} - {request.EndDate.ToLocalTime():dd.MM.yyyy}\n\n");
            sb.Append($"Кол-во денежных средств: {request.AccountBalance}\n\n");
            sb.Append("Траты по категориям:\n");
            foreach (var record in aggregation)
            {
                sb.Append($"{record.Category}: {record.Sum}\n");
                spent += record.Sum;
            }
            sb.Append("\n");
            sb.Append($"Потрачено денег: {spent}\n");
            sb.Append($"Остаток на балансе: {request.AccountBalance - spent}");
            
            return sb.ToString();
        }
    }
}