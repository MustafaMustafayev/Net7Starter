namespace CORE.Config;

public record SftpSettings
{
    public required string UserName { get; set; }
    public required string Ip { get; set; }
    public required string Password { get; set; }
}