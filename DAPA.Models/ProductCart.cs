using System.Text.Json.Serialization;

namespace DAPA.Models;

public class ProductCart
{
    public int OrderId { get; set; }

    [JsonIgnore] public Order Order { get; set; } = null!;

    public int ProductId { get; set; }

    [JsonIgnore] public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}