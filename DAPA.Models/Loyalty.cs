using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Loyalty
{
    public int Id { get; set; }

    public string? StartDate { get; set; }

    public int DiscountId { get; set; }

    public Discount Discount { get; set; } = null!;
}