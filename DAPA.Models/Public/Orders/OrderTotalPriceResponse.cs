namespace DAPA.Models.Public.Orders;

public class OrderTotalPriceResponse
{
    public float TotalAmount { get; set; }

    public float Paid { get; set; }

    public float Discount { get; set; }

    public float Tip { get; set; }

    public float Tax { get; set; }

    public float ToPay { get; set; }
}
