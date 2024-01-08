using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Products;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Products;

public class ProductCartDatabaseRepository : IProductCartRepository
{
    private readonly IOrderContext _context;

    public ProductCartDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductCart>> GetAllAsync()
    {
        return await _context.ProductCarts.ToListAsync();
    }

    public async Task<IEnumerable<ProductCart>> GetAllAsync(ProductCartFindRequest request)
    {
        var query = _context.ProductCarts.AsQueryable();

        if (request.OrderId.HasValue)
            query = query.Where(p => p.OrderId == request.OrderId);

        if (request.ProductId.HasValue)
            query = query.Where(p => p.ProductId == request.ProductId);

        if (request.Quantity.HasValue)
            query = query.Where(p => p.Quantity == request.Quantity);

        return await query.ToListAsync();
    }

    public async Task<ProductCart?> GetByPropertyAsync(Expression<Func<ProductCart, bool>> expression)
    {
        return await _context.ProductCarts.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<ProductCart, bool>> expression)
    {
        return await _context.ProductCarts.AnyAsync(expression);
    }

    public async Task InsertAsync(ProductCart entity)
    {
        await _context.ProductCarts.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductCart entity)
    {
        _context.ProductCarts.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductCart entity)
    {
        _context.ProductCarts.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}