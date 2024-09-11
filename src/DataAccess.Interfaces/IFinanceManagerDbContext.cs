using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces;

public interface IFinanceManagerDbContext
{
    DbSet<Expense> Expenses { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}