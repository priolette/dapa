using System.Text.Json.Serialization;

namespace DAPA.Models;

public class Staff
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int Password { get; set; }

    public string? Position { get; set; }

    public int RoleId { get; set; }

    [JsonIgnore] public Role Role { get; set; } = null!;
}