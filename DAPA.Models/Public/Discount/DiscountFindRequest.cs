namespace DAPA.Models.Public;

public class DiscountFindRequest
{
    public int? ID { get; set; }

    public string? Name { get; set; }

    public int? Size { get; set; }

    public string? Start_date { get; set; }

    public string? End_date { get; set; }

    public string? Applicable_Category { get; set; }
}
