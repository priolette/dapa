namespace DAPA.Models.Public.Reservations;

public class ReservationCreateRequest
{
    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public int StaffId { get; set; }

    public DateTime DateTime { get; set; }
}