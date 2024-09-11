using MediatR;
using UseCases.DTO;

namespace UseCases.Expenses.Commands;

public class SaveExpenseCommand : IRequest<long>
{
    public ExpenseDto ExpenseDto { get; set; }
}