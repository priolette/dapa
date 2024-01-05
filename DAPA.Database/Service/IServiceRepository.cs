using DAPA.Models;
using DAPA.Models.Public;

namespace DAPA.Database;

public interface IServiceRepository : IRepository<Service>
{
    public Task<IEnumerable<Service>> GetAllAsync(ServiceFindRequest request);
}