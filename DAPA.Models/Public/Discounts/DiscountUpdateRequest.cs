namespace DAPA.Models.Public.Discounts;

public class DiscountUpdateRequest
{
    public string? Name { get; set; }

    public int? Size { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? ApplicableCategory { get; set; }
}