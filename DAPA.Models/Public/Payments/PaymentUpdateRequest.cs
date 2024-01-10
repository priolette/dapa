namespace DAPA.Models.Public.Payments;

public class PaymentUpdateRequest
{
    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public Method Method { get; set; }
}