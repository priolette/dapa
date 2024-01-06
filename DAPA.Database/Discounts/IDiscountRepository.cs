using DAPA.Models.Public.Discounts;

namespace DAPA.Database.Discounts;

public interface IDiscountRepository : IRepository<Models.Discount>
{
    public Task<IEnumerable<Models.Discount>> GetAllAsync(DiscountFindRequest request);
}