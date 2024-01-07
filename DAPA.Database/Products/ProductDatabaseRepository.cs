using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Products;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Products;

public class ProductDatabaseRepository : IProductRepository
{
    private readonly IOrderContext _context;

    public ProductDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(ProductFindRequest request)
    {
        var query = _context.Products.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(x => x.Id == request.Id);

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.Title == request.Title);

        if (request.Price.HasValue)
            query = query.Where(x => x.Price == request.Price);

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(x => x.Description == request.Description);

        if (request.DiscountId.HasValue)
            query = query.Where(x => x.DiscountId == request.DiscountId);

        if (!string.IsNullOrWhiteSpace(request.Category))
            query = query.Where(x => x.Category == request.Category);

        return await query.ToListAsync();
    }

    public async Task<Product?> GetByPropertyAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.AnyAsync(expression);
    }

    public async Task InsertAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        _context.Products.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}