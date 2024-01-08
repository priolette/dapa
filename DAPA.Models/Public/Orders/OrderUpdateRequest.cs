namespace DAPA.Models.Public.Orders;

public class OrderUpdateRequest
{
    public int ClientId { get; set; }

    public DateTime CreationDate { get; set; }

    public Status Status { get; set; }

    public int StaffId { get; set; }

    public int DiscountId { get; set; }

    public string? Details { get; set; }
}