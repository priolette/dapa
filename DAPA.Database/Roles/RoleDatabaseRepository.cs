using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Roles;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Roles;

public class RoleDatabaseRepository : IRoleRepository
{
    private readonly IOrderContext _context;

    public RoleDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetAllAsync(RoleFindRequest request)
    {
        var query = _context.Roles.AsQueryable();

        if (request.Id.HasValue)
        {
            query = query.Where(x => x.Id == request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(x => x.Title == request.Title);
        }

        if (request.PermissionViewOrder.HasValue)
        {
            query = query.Where(x => x.PermissionViewOrder == request.PermissionViewOrder);
        }

        if (request.PermissionManageOrder.HasValue)
        {
            query = query.Where(x => x.PermissionManageOrder == request.PermissionManageOrder);
        }

        if (request.PermissionCreateDiscount.HasValue)
        {
            query = query.Where(x => x.PermissionCreateDiscount == request.PermissionCreateDiscount);
        }

        if (request.PermissionManageItems.HasValue)
        {
            query = query.Where(x => x.PermissionManageItems == request.PermissionManageItems);
        }

        if (request.PermissionManageServices.HasValue)
        {
            query = query.Where(x => x.PermissionManageServices == request.PermissionManageServices);
        }

        if (request.IsOwner.HasValue)
        {
            query = query.Where(x => x.IsOwner == request.IsOwner);
        }

        return await query.ToListAsync();
    }

    public async Task<Role?> GetByPropertyAsync(Expression<Func<Role, bool>> expression)
    {
        return await _context.Roles.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Role, bool>> expression)
    {
        return await _context.Roles.AnyAsync(expression);
    }

    public async Task InsertAsync(Role entity)
    {
        await _context.Roles.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpdateAsync(Role entity)
    {
        _context.Roles.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Role entity)
    {
        _context.Roles.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}