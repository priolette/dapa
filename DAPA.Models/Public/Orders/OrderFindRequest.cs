namespace DAPA.Models.Public.Orders;

public class OrderFindRequest
{
    public int? Id { get; set; }

    public int? ClientId { get; set; }

    public DateTime? CreationDate { get; set; }

    public Status? Status { get; set; }
}