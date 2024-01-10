namespace DAPA.Models.Public.WorkingHours;

public class WorkingHoursUpdateRequest
{
    public int StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}