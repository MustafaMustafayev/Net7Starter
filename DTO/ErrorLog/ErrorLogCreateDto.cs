namespace DTO.ErrorLog;

public record ErrorLogCreateDto
{
    public string? AccessToken { get; set; }
    public Guid? UserId { get; set; }
    public string? Path { get; set; }
    public string? Ip { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
}