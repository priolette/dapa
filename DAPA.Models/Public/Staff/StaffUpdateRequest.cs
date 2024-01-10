namespace DAPA.Models.Public.Staff;

public class StaffUpdateRequest
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int Password { get; set; }

    public string? Position { get; set; }

    public int? RoleId { get; set; }
}