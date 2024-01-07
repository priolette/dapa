namespace DAPA.Models.Public.Reservations;

public class ReservationUpdateRequest
{
    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public int StaffId { get; set; }

    public DateTime DateTime { get; set; }
}