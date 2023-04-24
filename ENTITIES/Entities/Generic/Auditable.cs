namespace ENTITIES.Entities.Generic;

public class Auditable
{
    public int Id { get; set; }
    public int? CreatedById { get; set; }
    public int? ModifiedBy { get; set; }
    public int? DeletedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}