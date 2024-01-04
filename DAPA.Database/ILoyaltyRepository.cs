using DAPA.Models;
using DAPA.Models.Public;

namespace DAPA.Database;

public interface ILoyaltyRepository: IRepository<Loyalty>
{
    public Task<IEnumerable<Loyalty>> GetAllAsync(LoyaltyFindRequest request);
}