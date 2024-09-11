using FluentValidation;
using UseCases.DTO;

namespace UseCases.Validators;

public class ExpenseValidator : AbstractValidator<ExpenseDto>
{
    public ExpenseValidator()
    {
        RuleFor(e => e.Value > 0).NotEmpty();
        RuleFor(e => e.Category).NotEmpty();
        RuleFor(e => e.DateTime).NotEmpty();
        RuleFor(e => e.ChannelId).NotEmpty();
        RuleFor(e => e.MessageId).NotEmpty();
    }
}