namespace ENTITIES.Entities;

public class Token : IEntity
{
    public int TokenId { get; set; }

    public virtual User User { get; set; }
    public int UserId { get; set; }
    public string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpireDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpireDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}