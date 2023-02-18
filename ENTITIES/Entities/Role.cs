namespace ENTITIES.Entities;

public class Role : IEntity
{
    public int RoleId { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public virtual List<Permission>? Permissions { get; set; }
}