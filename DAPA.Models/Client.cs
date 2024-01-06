using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int LoyaltyId { get; set; }

    [JsonIgnore] public Loyalty Loyalty { get; set; } = null!;

    public int PhoneNumber { get; set; }

    public string? Email { get; set; }
}