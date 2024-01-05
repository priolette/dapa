using DAPA.Models;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public interface IOrderContext
{
    DbSet<Discount> Discounts { get; }
    DbSet<Loyalty> Loyalties { get; }
    DbSet<Service> Services { get; }
    DbContext Instance { get; }
}

public class OrderContext : DbContext, IOrderContext
{
    public DbSet<Discount> Discounts { get; private set; } = null!;
    public DbSet<Loyalty> Loyalties { get; private set; } = null!;
    public DbSet<Service> Services { get; private set; } = null!;
    public DbContext Instance => this;

    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }
}