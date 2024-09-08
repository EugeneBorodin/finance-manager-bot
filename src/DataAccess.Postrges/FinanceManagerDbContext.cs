using DataAccess.Interfaces;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postrges;

public sealed class FinanceManagerDbContext : DbContext, IFinanceManagerDbContext
{
    public DbSet<Message> Messages { get; set; }

    public FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}