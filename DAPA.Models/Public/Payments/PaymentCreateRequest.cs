namespace DAPA.Models.Public.Payments;

public class PaymentCreateRequest
{
    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public Method Method { get; set; }

    public float? Amount { get; set; }
}