using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public class LoyaltyDatabaseRepository : ILoyaltyRepository
{
    private readonly IOrderContext _context;

    public LoyaltyDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Loyalty>> GetAllAsync()
    {
        return await _context.Loyalties.ToListAsync();
    }

    public async Task<IEnumerable<Loyalty>> GetAllAsync(LoyaltyFindRequest request)
    {
        var query = _context.Loyalties.AsQueryable();

        if (request.ID.HasValue)
            query = query.Where(l => l.Id == request.ID.Value);

        if (!string.IsNullOrEmpty(request.StartDate))
            query = query.Where(l => l.StartDate == request.StartDate);

        if (request.Discount.HasValue)
            query = query.Where(l => l.DiscountId == request.Discount);

        return await query.ToListAsync();
    }

    public async Task<Loyalty?> GetByPropertyAsync(Expression<Func<Loyalty, bool>> expression)
    {
        return await _context.Loyalties.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Loyalty, bool>> expression)
    {
        return await _context.Loyalties.AnyAsync(expression);
    }

    public async Task InsertAsync(Loyalty entity)
    {
        await _context.Loyalties.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Loyalty entity)
    {
        _context.Loyalties.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Loyalty entity)
    {
        _context.Loyalties.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}