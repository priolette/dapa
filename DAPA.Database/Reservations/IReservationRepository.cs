using DAPA.Models;
using DAPA.Models.Public.Reservations;

namespace DAPA.Database.Reservations;

public interface IReservationRepository : IRepository<Reservation>
{
    public Task<IEnumerable<Reservation>> GetAllAsync(ReservationFindRequest request);
}