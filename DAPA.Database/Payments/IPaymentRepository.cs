using DAPA.Models;
using DAPA.Models.Public.Payments;

namespace DAPA.Database.Payments;

public interface IPaymentRepository : IRepository<Payment>
{
    public Task<IEnumerable<Payment>> GetAllAsync(PaymentFindRequest request);
}