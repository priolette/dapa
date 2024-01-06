using System.Linq.Expressions;
using DAPA.Models.Public.Staff;

namespace DAPA.Database.Staff;

public class StaffDatabaseRepository : IStaffRepository
{
    public async Task<IEnumerable<Models.Staff>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Models.Staff>> GetAllAsync(StaffCreateRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Models.Staff?> GetByPropertyAsync(Expression<Func<Models.Staff, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByPropertyAsync(Expression<Func<Models.Staff, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task InsertAsync(Models.Staff entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Models.Staff entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Models.Staff entity)
    {
        throw new NotImplementedException();
    }
}