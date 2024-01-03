using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Token : IEntity
{
    [Key] public Guid TokenId { get; set; }
    public virtual required User User { get; set; }
    public Guid UserId { get; set; }
    public required string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpireDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}