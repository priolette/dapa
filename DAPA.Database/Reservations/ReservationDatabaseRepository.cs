using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Reservations;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Reservations;

public class ReservationDatabaseRepository : IReservationRepository
{
    private readonly IOrderContext _context;

    public ReservationDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync(ReservationFindRequest request)
    {
        var query = _context.Reservations.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(r => r.Id == request.Id);

        if (request.ClientId.HasValue)
            query = query.Where(r => r.ClientId == request.ClientId);

        if (request.ServiceId.HasValue)
            query = query.Where(r => r.ServiceId == request.ServiceId);

        if (request.StaffId.HasValue)
            query = query.Where(r => r.StaffId == request.StaffId);

        if (request.DateTime.HasValue)
            query = query.Where(r => r.DateTime == request.DateTime);

        return await query.ToListAsync();
    }

    public async Task<Reservation?> GetByPropertyAsync(Expression<Func<Reservation, bool>> expression)
    {
        return await _context.Reservations.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Reservation, bool>> expression)
    {
        return await _context.Reservations.AnyAsync(expression);
    }

    public async Task InsertAsync(Reservation entity)
    {
        await _context.Reservations.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation entity)
    {
        _context.Reservations.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reservation entity)
    {
        _context.Reservations.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}