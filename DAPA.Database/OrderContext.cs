using DAPA.Models;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public interface IOrderContext
{
    DbSet<Discount> Discounts { get; }
    DbSet<Loyalty> Loyalties { get; }
    DbSet<Service> Services { get; }
    DbSet<Client> Clients { get; }
    DbSet<Models.Staff> Staff { get; }
    DbSet<Role> Roles { get; }
    DbSet<Product> Products { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<Order> Orders { get; }
    DbSet<ProductCart> ProductCarts { get; }
    DbSet<ServiceCart> ServiceCarts { get; }
    DbSet<Payment> Payments { get; }
        DbSet<WorkingHour> WorkingHours { get; }
    DbContext Instance { get; }
}

public class OrderContext : DbContext, IOrderContext
{
    public DbSet<Discount> Discounts { get; private set; } = null!;
    public DbSet<Loyalty> Loyalties { get; private set; } = null!;
    public DbSet<Service> Services { get; private set; } = null!;
    public DbSet<Client> Clients { get; private set; } = null!;
    public DbSet<Models.Staff> Staff { get; private set; } = null!;
    public DbSet<Role> Roles { get; private set; } = null!;
    public DbSet<Product> Products { get; private set; } = null!;
    public DbSet<Reservation> Reservations { get; private set; } = null!;
    public DbSet<Order> Orders { get; private set; } = null!;
    public DbSet<ProductCart> ProductCarts { get; private set; } = null!;
    public DbSet<ServiceCart> ServiceCarts { get; private set; } = null!;
    public DbSet<Payment> Payments { get; private set; } = null!;
    public DbSet<WorkingHour> WorkingHours { get; private set; } = null!;

    public DbContext Instance => this;

    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCart>()
            .HasKey(pc => new { pc.OrderId, pc.ProductId });

        modelBuilder.Entity<ServiceCart>()
            .HasKey(sc => new { sc.OrderId, sc.ServiceId });

        modelBuilder.Entity<Client>()
            .HasIndex(u => new { u.Name, u.Surname })
            .IsUnique();
    }
}