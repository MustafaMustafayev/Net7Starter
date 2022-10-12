using Project.DTO.DTOs.CustomLoggingDto;

namespace Project.BLL.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDto requestLogDto);
}