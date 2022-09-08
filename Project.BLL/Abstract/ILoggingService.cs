using Project.DTO.DTOs.CustomLoggingDTOs;

namespace Project.BLL.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDTO requestLogDto);
}