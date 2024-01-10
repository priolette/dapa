using DAPA.Models;
using DAPA.Models.Public.Services;
using DAPA.Models.Public.WorkingHours;

namespace DAPA.Database.WorkingHours;

public interface IWorkingHoursRepository : IRepository<WorkingHour>
{
    public Task<IEnumerable<WorkingHour>> GetAllAsync(WorkingHoursFindRequest request);
}