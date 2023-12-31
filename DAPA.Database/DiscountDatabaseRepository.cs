using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public class DiscountDatabaseRepository : IDiscountRepository
{
    private readonly IDiscountContext _context;

    public DiscountDatabaseRepository(IDiscountContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Discount>> GetAllAsync()
    {
        return await _context.Discounts.ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetAllAsync(DiscountFindRequest request)
    {
        var query = _context.Discounts.AsQueryable();

        if (request.ID.HasValue)
            query = query.Where(d => d.ID == request.ID.Value);

        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(d => d.Name == request.Name);

        if (request.Size.HasValue)
            query = query.Where(d => d.Size == request.Size.Value);

        if (!string.IsNullOrEmpty(request.Start_date))
            query = query.Where(d => d.Start_date == request.Start_date);

        if (!string.IsNullOrEmpty(request.End_date))
            query = query.Where(d => d.End_date == request.End_date);

        if (!string.IsNullOrEmpty(request.Applicable_Category))
            query = query.Where(d => d.Applicable_Category == request.Applicable_Category);

        return await query.ToListAsync();
    }

    public Task<Discount?> GetByPropertyAsync(Expression<Func<Discount, bool>> expression)
    {
        return _context.Discounts.FirstOrDefaultAsync(expression);
    }

    public Task<bool> ExistsByPropertyAsync(Expression<Func<Discount, bool>> expression)
    {
        return _context.Discounts.AnyAsync(expression);
    }

    public async Task InsertAsync(Discount entity)
    {
        await _context.Discounts.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Discount entity)
    {
        _context.Discounts.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Discount entity)
    {
        _context.Discounts.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}
