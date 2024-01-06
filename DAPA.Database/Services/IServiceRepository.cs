using DAPA.Models;
using DAPA.Models.Public.Services;

namespace DAPA.Database.Services;

public interface IServiceRepository : IRepository<Service>
{
    public Task<IEnumerable<Service>> GetAllAsync(ServiceFindRequest request);
}