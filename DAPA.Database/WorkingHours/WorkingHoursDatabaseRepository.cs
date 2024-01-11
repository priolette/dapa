using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.WorkingHours;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.WorkingHours;

public class WorkingHoursDatabaseRepository : IWorkingHoursRepository
{
    private readonly IOrderContext _context;

    public WorkingHoursDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkingHour>> GetAllAsync()
    {
        return await _context.WorkingHours.ToListAsync();
    }

    public async Task<IEnumerable<WorkingHour>> GetAllAsync(WorkingHoursFindRequest request)
    {
        var query = _context.WorkingHours.AsQueryable();

        if (request.Id.HasValue)
        {
            query = query.Where(x => x.Id == request.Id.Value);
        }

        if (request.StaffId.HasValue)
        {
            query = query.Where(x => x.StaffId == request.StaffId.Value);
        }

        if (request.StartTime.HasValue)
        {
            query = query.Where(x => x.StartTime == request.StartTime.Value);
        }

        if (request.EndTime.HasValue)
        {
            query = query.Where(x => x.EndTime == request.EndTime.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<WorkingHour?> GetByPropertyAsync(Expression<Func<WorkingHour, bool>> expression)
    {
        return await _context.WorkingHours.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<WorkingHour, bool>> expression)
    {
        return await _context.WorkingHours.AnyAsync(expression);
    }

    public async Task InsertAsync(WorkingHour entity)
    {
        await _context.WorkingHours.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(WorkingHour entity)
    {
        _context.WorkingHours.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(WorkingHour entity)
    {
        _context.WorkingHours.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }

}
