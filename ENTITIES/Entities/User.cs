namespace ENTITIES.Entities;

public class User : Auditable
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string ContactNumber { get; set; }
    public required string Password { get; set; }
    public required string Salt { get; set; }
    public string? LastVerificationCode { get; set; }
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public int? ImageId { get; set; }
    public virtual File? Image { get; set; }
}