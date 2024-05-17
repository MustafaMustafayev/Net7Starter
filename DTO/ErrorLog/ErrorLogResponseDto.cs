namespace DTO.ErrorLog;

public record ErrorLogResponseDto
{
    public Guid Id { get; set; }
    public string DateTime { get; set; } = default!;
    public string? AccessToken { get; set; }
    public Guid? UserId { get; set; }
    public string? Path { get; set; }
    public string? Ip { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
}