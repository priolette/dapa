namespace DAPA.Models.Public.Services;

public class ServiceCartFindRequest
{
    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }
}