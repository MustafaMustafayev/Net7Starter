namespace CORE.Config;

public record MailSettings
{
    public required string Address { get; set; }
    public required string DisplayName { get; set; }
    public required string MailKey { get; set; }
    public required string Subject { get; set; }
    public required string Host { get; set; }
    public required string Port { get; set; }
}