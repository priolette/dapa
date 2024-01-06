using DAPA.Models.Public.Staff;

namespace DAPA.Database.Staff;

public interface IStaffRepository : IRepository<Models.Staff>
{
    public Task<IEnumerable<Models.Staff>> GetAllAsync(StaffCreateRequest request);
}