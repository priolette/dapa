using DAPA.Models;
using DAPA.Models.Public;

namespace DAPA.Database;

public interface IDiscountRepository : IRepository<Discount>
{
    public Task<IEnumerable<Discount>> GetAllAsync(DiscountFindRequest request);
}
