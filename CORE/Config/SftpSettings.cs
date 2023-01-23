namespace CORE.Config;

public record SftpSettings
{
    public required string UserName { get; set; }
    public required string IP { get; set; }
    public required string Password { get; set; }
}