namespace CORE.Config
{
    public record MailSettings
    {
        public string Address { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string MailKey { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Host { get; set; } = default!;
        public string Port { get; set; } = default!;
    }
}
