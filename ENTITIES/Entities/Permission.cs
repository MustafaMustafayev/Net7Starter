namespace ENTITIES.Entities;

public class Permission : Auditable, IEntity
{
    public int PermissionId { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public virtual List<Role> Roles { get; set; }
}