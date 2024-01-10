using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Payments;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Payments;

public class PaymentDatabaseRepository : IPaymentRepository
{
    private readonly IOrderContext _context;

    public PaymentDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(PaymentFindRequest request)
    {
        var query = _context.Payments.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(x => x.Id == request.Id.Value);

        if (request.OrderId.HasValue)
            query = query.Where(x => x.OrderId == request.OrderId.Value);

        if (request.Date.HasValue)
            query = query.Where(x => x.Date == request.Date.Value);

        if (request.Method.HasValue)
            query = query.Where(x => x.Method == request.Method.Value);

        return await query.ToListAsync();
    }

    public async Task<Payment?> GetByPropertyAsync(Expression<Func<Payment, bool>> expression)
    {
        return await _context.Payments.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Payment, bool>> expression)
    {
        return await _context.Payments.AnyAsync(expression);
    }

    public async Task InsertAsync(Payment entity)
    {
        await _context.Payments.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment entity)
    {
        _context.Payments.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Payment entity)
    {
        _context.Payments.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}