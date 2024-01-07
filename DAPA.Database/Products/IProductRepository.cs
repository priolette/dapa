using DAPA.Models;
using DAPA.Models.Public.Products;

namespace DAPA.Database.Products;

public interface IProductRepository : IRepository<Product>
{
    public Task<IEnumerable<Product>> GetAllAsync(ProductFindRequest request);
}