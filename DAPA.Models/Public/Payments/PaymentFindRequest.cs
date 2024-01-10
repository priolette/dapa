namespace DAPA.Models.Public.Payments;

public class PaymentFindRequest
{
    public int? Id { get; set; }

    public int? OrderId { get; set; }

    public DateTime? Date { get; set; }

    public Method? Method { get; set; }
}