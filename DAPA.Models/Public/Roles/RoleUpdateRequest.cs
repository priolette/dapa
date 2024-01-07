namespace DAPA.Models.Public.Roles;

public class RoleUpdateRequest
{
    public string? Title { get; set; }

    public bool PermissionViewOrder { get; set; }

    public bool PermissionManageOrder { get; set; }

    public bool PermissionCreateDiscount { get; set; }

    public bool PermissionManageItems { get; set; }

    public bool PermissionManageServices { get; set; }

    public bool IsOwner { get; set; }
}