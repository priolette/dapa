using System.Linq.Expressions;
using DAPA.Models;
using DAPA.Models.Public.Clients;
using Microsoft.EntityFrameworkCore;

namespace DAPA.Database.Clients;

public class ClientDatabaseRepository : IClientRepository
{
    private readonly IOrderContext _context;

    public ClientDatabaseRepository(IOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<IEnumerable<Client>> GetAllAsync(ClientFindRequest request)
    {
        var query = _context.Clients.AsQueryable();

        if (request.Id.HasValue)
            query = query.Where(c => c.Id == request.Id.Value);

        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(c => c.Name == request.Name);

        if (!string.IsNullOrEmpty(request.Email))
            query = query.Where(c => c.Email == request.Email);

        if (request.PhoneNumber.HasValue)
            query = query.Where(c => c.PhoneNumber == request.PhoneNumber.Value);

        return await query.ToListAsync();
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Client, bool>> predicate)
    {
        return await _context.Clients.AnyAsync(predicate);
    }

    public async Task InsertAsync(Client entity)
    {
        await _context.Clients.AddAsync(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task<Client?> GetByPropertyAsync(Expression<Func<Client, bool>> predicate)
    {
        return await _context.Clients.FirstOrDefaultAsync(predicate);
    }

    public async Task UpdateAsync(Client entity)
    {
        _context.Clients.Update(entity);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(Client entity)
    {
        _context.Clients.Remove(entity);
        await _context.Instance.SaveChangesAsync();
    }
}