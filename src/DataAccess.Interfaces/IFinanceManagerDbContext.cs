using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces;

public interface IFinanceManagerDbContext
{
    DbSet<Message> Messages { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}