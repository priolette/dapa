namespace DAPA.Models.Public;

public class DiscountUpdateRequest
{
    public string? Name { get; set; }

    public int? Size { get; set; }

    public string? Start_date { get; set; }

    public string? End_date { get; set; }

    public string? Applicable_Category { get; set; }
}