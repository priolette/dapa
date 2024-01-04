using DAPA.Models;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public interface ILoyaltyContext
{
    DbSet<Loyalty> Loyalties { get; }
    DbContext Instance { get; }
}

public class LoyaltyContext: DbContext, ILoyaltyContext
{
    public DbSet<Loyalty> Loyalties { get; private set; } = null!;
    public DbContext Instance => this;

    public LoyaltyContext(DbContextOptions<LoyaltyContext> options)
        : base(options) { }
}