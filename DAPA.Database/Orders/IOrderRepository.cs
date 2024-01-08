using DAPA.Models;
using DAPA.Models.Public.Orders;

namespace DAPA.Database.Orders;

public interface IOrderRepository : IRepository<Order>
{
    public Task<IEnumerable<Order>> GetAllAsync(OrderFindRequest request);
}