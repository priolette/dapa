using DAPA.Models;
using DAPA.Models.Public.Clients;

namespace DAPA.Database.Clients;

public interface IClientRepository : IRepository<Client>
{
    public Task<IEnumerable<Client>> GetAllAsync(ClientFindRequest request);
}