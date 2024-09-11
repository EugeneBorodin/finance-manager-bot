using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Models;

public class Expense
{
    [Key]
    public long Id { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public decimal Value { get; set; }
    public string Category { get; set; }
    public long MessageId { get; set; }
    public long ChannelId { get; set; }
}