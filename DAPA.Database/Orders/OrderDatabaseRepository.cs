using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Orders;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Orders;

public class OrderDatabaseRepository : IOrderRepository
{
    private readonly IOrderContext _context;

    public OrderDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync(OrderFindRequest request)
    {
        var query = _context.Orders.AsQueryable();

        if (request.ClientId.HasValue)
            query = query.Where(x => x.ClientId == request.ClientId.Value);

        if (request.CreationDate.HasValue)
            query = query.Where(x => x.CreationDate == request.CreationDate.Value);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        return await query.ToListAsync();
    }

    public async Task<Order?> GetByPropertyAsync(Expression<Func<Order, bool>> expression)
    {
        return await _context.Orders.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Order, bool>> expression)
    {
        return await _context.Orders.AnyAsync(expression);
    }

    public async Task InsertAsync(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        _context.Orders.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order entity)
    {
        _context.Orders.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}