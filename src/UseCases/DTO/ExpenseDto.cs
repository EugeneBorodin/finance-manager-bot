namespace UseCases.DTO;

public class ExpenseDto
{
    public DateTimeOffset DateTime { get; set; }
    public decimal Value { get; set; }
    public string Category { get; set; }
    public long MessageId { get; set; }
    public long ChannelId { get; set; }
}