using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Discounts;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Discounts;

public class DiscountDatabaseRepository : IDiscountRepository
{
    private readonly IOrderContext _context;

    public DiscountDatabaseRepository(IOrderContext context)
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

        if (request.Id.HasValue)
            query = query.Where(d => d.Id == request.Id.Value);

        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(d => d.Name == request.Name);

        if (request.Size.HasValue)
            query = query.Where(d => d.Size == request.Size.Value);

        if (!string.IsNullOrEmpty(request.StartDate))
            query = query.Where(d => d.StartDate == request.StartDate);

        if (!string.IsNullOrEmpty(request.EndDate))
            query = query.Where(d => d.EndDate == request.EndDate);

        if (!string.IsNullOrEmpty(request.ApplicableCategory))
            query = query.Where(d => d.ApplicableCategory == request.ApplicableCategory);

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