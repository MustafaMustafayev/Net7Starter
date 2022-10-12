using Project.DTO.CustomLogging;

namespace Project.BLL.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDto requestLogDto);
}