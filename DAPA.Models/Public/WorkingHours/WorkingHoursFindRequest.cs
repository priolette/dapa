namespace DAPA.Models.Public.WorkingHours;

public class WorkingHoursFindRequest
{
    public int? Id { get; set; }

    public int? StaffId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}