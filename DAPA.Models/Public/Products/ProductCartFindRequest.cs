namespace DAPA.Models.Public.Products;

public class ProductCartFindRequest
{
    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }
}