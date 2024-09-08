using FluentValidation;
using UseCases.Messages.DTO;

namespace UseCases.Validators;

public class MessageValidator : AbstractValidator<MessageDto>
{
    public MessageValidator()
    {
        RuleFor(message => message.MessageId).NotEqual(0);
        RuleFor(message => message.ChatId).NotEqual(0);
        RuleFor(message => message.Text).NotEmpty();
        RuleFor(message => message.DateTime).NotNull();
    }
}