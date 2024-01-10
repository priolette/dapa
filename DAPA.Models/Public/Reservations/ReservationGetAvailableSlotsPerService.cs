namespace DAPA.Models.Public.Reservations;

public class ReservationGetAvailableSlotsPerService
{
    public int StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}