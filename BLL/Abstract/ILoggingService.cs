using DTO.Logging;

namespace BLL.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDto dto);
}