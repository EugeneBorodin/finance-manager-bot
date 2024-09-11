using DataAccess.Interfaces;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postrges;

public sealed class FinanceManagerDbContext : DbContext, IFinanceManagerDbContext
{
    public DbSet<Expense> Expenses { get; set; }

    public FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}