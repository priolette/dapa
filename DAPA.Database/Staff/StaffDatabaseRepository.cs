using System.Linq.Expressions;
using DAPA.Models.Public.Staff;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Staff;

public class StaffDatabaseRepository : IStaffRepository
{
    private readonly IOrderContext _context;

    public StaffDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.Staff>> GetAllAsync()
    {
        return await _context.Staff.ToListAsync();
    }

    public async Task<IEnumerable<Models.Staff>> GetAllAsync(StaffFindRequest request)
    {
        var query = _context.Staff.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(s => s.Id == request.Id);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(s => s.Name == request.Name);

        if (!string.IsNullOrWhiteSpace(request.Surname))
            query = query.Where(s => s.Surname == request.Surname);

        if (request.Password.HasValue)
            query = query.Where(s => s.Password == request.Password);

        if (!string.IsNullOrWhiteSpace(request.Position))
            query = query.Where(s => s.Position == request.Position);

        if (request.RoleId.HasValue)
            query = query.Where(s => s.RoleId == request.RoleId);

        return await query.ToListAsync();
    }

    public async Task<Models.Staff?> GetByPropertyAsync(Expression<Func<Models.Staff, bool>> expression)
    {
        return await _context.Staff.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Models.Staff, bool>> expression)
    {
        return await _context.Staff.AnyAsync(expression);
    }

    public async Task InsertAsync(Models.Staff entity)
    {
        await _context.Staff.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Models.Staff entity)
    {
        _context.Staff.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Models.Staff entity)
    {
        _context.Staff.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}