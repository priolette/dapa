using DAPA.Models;
using Microsoft.EntityFrameworkCore;

namespace DAPA.DataAccess;

public interface IDiscountContext
{
    DbSet<Discount> Discounts { get; }
    DbContext Instance { get; }
}

public class DiscountContext : DbContext, IDiscountContext
{
    public DbSet<Discount> Discounts { get; private set; } = null!;
    public DbContext Instance => this;

    public DiscountContext(DbContextOptions<DiscountContext> options)
        : base(options) { }
}
