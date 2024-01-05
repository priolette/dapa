namespace DAPA.Models.Public;

public class ServiceFindRequest
{
    public int? Id { get; set; }

    public string? Title { get; set; }

    public float? Price { get; set; }

    public int? Duration { get; set; }

    public string? Category { get; set; }
}