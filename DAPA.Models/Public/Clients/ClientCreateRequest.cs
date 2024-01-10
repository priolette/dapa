namespace DAPA.Models.Public.Clients;

public class ClientCreateRequest
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int? LoyaltyId { get; set; }

    public int PhoneNumber { get; set; }

    public string? Email { get; set; }
}