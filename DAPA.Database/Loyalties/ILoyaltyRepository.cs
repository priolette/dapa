using DAPA.Models;
using DAPA.Models.Public.Loyalties;

namespace DAPA.Database.Loyalties;

public interface ILoyaltyRepository : IRepository<Loyalty>
{
    public Task<IEnumerable<Loyalty>> GetAllAsync(LoyaltyFindRequest request);
}