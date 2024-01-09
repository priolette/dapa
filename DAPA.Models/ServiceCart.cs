using System.Text.Json.Serialization;

namespace DAPA.Models;

public class ServiceCart
{
    public int OrderId { get; set; }

    [JsonIgnore] public Order Order { get; set; } = null!;

    public int ServiceId { get; set; }

    [JsonIgnore] public Service Service { get; set; } = null!;

    public int? StaffId { get; set; }

    [JsonIgnore] public Staff Staff { get; set; } = null!;

    public int Quantity { get; set; }
}