namespace DAPA.Models.Public.Staff;

public class StaffCreateRequest
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int Password { get; set; }

    public string? Position { get; set; }

    public int? RoleId { get; set; }
}