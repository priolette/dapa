namespace DAPA.Models.Public.WorkingHours;

public class WorkingHoursCreateRequest
{
    public int StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}