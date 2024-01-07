using DAPA.Models;
using DAPA.Models.Public.Roles;

namespace DAPA.Database.Roles;

public interface IRoleRepository : IRepository<Role>
{
    public Task<IEnumerable<Role>> GetAllAsync(RoleFindRequest request);
}