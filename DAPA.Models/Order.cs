using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Order
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    [JsonIgnore] public Client Client { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Status Status { get; set; }

    public int StaffId { get; set; }

    [JsonIgnore] public Staff Staff { get; set; } = null!;

    public int? DiscountId { get; set; }

    [JsonIgnore] public Discount Discount { get; set; } = null!;

    public string? Details { get; set; }

    public float Tip { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Cancelled,
    Completed,
    InProgress,
    Paid
}