using MediatR;
using UseCases.Messages.DTO;

namespace UseCases.Messages.Commands;

public class SaveMessageCommand : IRequest<Guid>
{
    public MessageDto MessageDto { get; set; }
}