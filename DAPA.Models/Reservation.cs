using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Reservation
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    [JsonIgnore] public Client Client { get; set; } = null!;

    public int ServiceId { get; set; }

    [JsonIgnore] public Service Service { get; set; } = null!;

    public int StaffId { get; set; }

    [JsonIgnore] public Staff Staff { get; set; } = null!;

    public DateTime DateTime { get; set; }
}