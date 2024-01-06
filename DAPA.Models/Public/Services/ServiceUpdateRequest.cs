namespace DAPA.Models.Public.Services;

public class ServiceUpdateRequest
{
    public string? Title { get; set; }

    public float? Price { get; set; }

    public int? Duration { get; set; }

    public string? Description { get; set; }

    public int? DiscountId { get; set; }

    public string? Category { get; set; }

    public int? PerformerId { get; set; }

    public DateTime ServiceDateTime { get; set; }
}