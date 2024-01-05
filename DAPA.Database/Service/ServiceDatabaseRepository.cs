using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database;

public class ServiceDatabaseRepository : IServiceRepository
{
    private readonly IOrderContext _context;

    public ServiceDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetAllAsync(ServiceFindRequest request)
    {
        var query = _context.Services.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(s => s.Id == request.Id.Value);

        if (!string.IsNullOrEmpty(request.Title))
            query = query.Where(s => s.Title == request.Title);

        if (request.Price.HasValue)
            query = query.Where(s => s.Price == request.Price.Value);

        if (request.Duration.HasValue)
            query = query.Where(s => s.Duration == request.Duration.Value);

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(s => s.Category == request.Category);

        return await query.ToListAsync();
    }

    public async Task<Service?> GetByPropertyAsync(Expression<Func<Service, bool>> expression)
    {
        return await _context.Services.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Service, bool>> expression)
    {
        return await _context.Services.AnyAsync(expression);
    }

    public async Task InsertAsync(Service entity)
    {
        await _context.Services.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service entity)
    {
        _context.Services.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Service entity)
    {
        _context.Services.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}