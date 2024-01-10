using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public float Price { get; set; }

    public string? Description { get; set; }

    public int? DiscountId { get; set; }

    [JsonIgnore] public Discount Discount { get; set; } = null!;

    public string? Category { get; set; }
}