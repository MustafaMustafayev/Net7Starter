using DTO.CustomLogging;

namespace BLL.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDto dto);
}