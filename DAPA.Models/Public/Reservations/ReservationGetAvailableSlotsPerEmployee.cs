namespace DAPA.Models.Public.Reservations;

public class ReservationGetAvailableSlotsPerEmployee
{
    public int StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}