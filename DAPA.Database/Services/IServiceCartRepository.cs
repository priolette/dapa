using DAPA.Models;
using DAPA.Models.Public.Services;

namespace DAPA.Database.Services;

public interface IServiceCartRepository : IRepository<ServiceCart>
{
    public Task<IEnumerable<ServiceCart>> GetAllAsync(ServiceCartFindRequest request);
}