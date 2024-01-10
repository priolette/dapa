using DAPA.Models;
using DAPA.Models.Public.Products;

namespace DAPA.Database.Products;

public interface IProductCartRepository : IRepository<ProductCart>
{
    public Task<IEnumerable<ProductCart>> GetAllAsync(ProductCartFindRequest request);
}