using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Services;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Services;

public class ServiceCartDatabaseRepository : IServiceCartRepository
{
    private readonly IOrderContext _context;

    public ServiceCartDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceCart>> GetAllAsync()
    {
        return await _context.ServiceCarts.ToListAsync();
    }

    public async Task<IEnumerable<ServiceCart>> GetAllAsync(ServiceCartFindRequest request)
    {
        var query = _context.ServiceCarts.AsQueryable();

        if (request.OrderId.HasValue)
            query = query.Where(sc => sc.OrderId == request.OrderId.Value);

        if (request.ServiceId.HasValue)
            query = query.Where(sc => sc.ServiceId == request.ServiceId.Value);

        if (request.Quantity.HasValue)
            query = query.Where(sc => sc.Quantity == request.Quantity.Value);

        return await query.ToListAsync();
    }

    public async Task<ServiceCart?> GetByPropertyAsync(Expression<Func<ServiceCart, bool>> expression)
    {
        return await _context.ServiceCarts.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<ServiceCart, bool>> expression)
    {
        return await _context.ServiceCarts.AnyAsync(expression);
    }

    public async Task InsertAsync(ServiceCart entity)
    {
        await _context.ServiceCarts.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(ServiceCart entity)
    {
        _context.ServiceCarts.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(ServiceCart entity)
    {
        _context.ServiceCarts.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}