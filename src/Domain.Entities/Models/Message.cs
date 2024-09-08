using Domain.Entities.Enums;

namespace Domain.Entities.Models;

public class Message
{
    public Guid Id { get; set; }
    public long MessageId { get; set; }
    public long ChatId { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public long SenderId { get; set; }
    public string SenderUsername { get; set; }
    public Status Status { get; set; }
}