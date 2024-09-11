using FluentValidation;
using UseCases.Expenses.Commands;

namespace UseCases.Validators;

public class CalculateSummaryCommandValidator : AbstractValidator<CalculateSummaryCommand>
{
    public CalculateSummaryCommandValidator()
    {
        RuleFor(command => command.ChannelId).NotEmpty();
        RuleFor(command => command.StartDate).NotEmpty();
        RuleFor(command => command.EndDate).NotEmpty();
        RuleFor(command => command.EndDate >= command.StartDate);
        RuleFor(command => command.AccountBalance > 0);
    }
}