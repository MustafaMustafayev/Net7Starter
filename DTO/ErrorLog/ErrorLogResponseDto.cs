namespace DTO.ErrorLog;

public record ErrorLogResponseDto
{
    public Guid ErrorLogId { get; set; }
    public DateTime DateTime { get; set; }
    public string AccessToken { get; set; }
    public Guid? UserId { get; set; }
    public string Path { get; set; }
    public string Ip { get; set; }
    public string ErrorMessage { get; set; }
    public string StackTrace { get; set; }
}