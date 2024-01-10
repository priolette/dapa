using System.Text.Json.Serialization;

namespace DAPA.Models;

public class WorkingHour
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}