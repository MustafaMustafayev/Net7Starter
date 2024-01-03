namespace DTO.ErrorLog;

public record ErrorLogToAddDto(
    string AccessToken,
    Guid? UserId,
    string Path,
    string Ip,
    string ErrorMessage,
    string StackTrace
    );

public record ErrorLogToListDto(
    Guid ErrorLogId,
    DateTime DateTime,
    string AccessToken,
    Guid? UserId,
    string Path,
    string Ip,
    string ErrorMessage,
    string StackTrace
    );
