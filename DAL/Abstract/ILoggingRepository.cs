using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities.Logging;

namespace DAL.Abstract;

public interface ILoggingRepository : IGenericRepository<RequestLog>
{
    Task AddLogAsync(RequestLog requestLog);
}