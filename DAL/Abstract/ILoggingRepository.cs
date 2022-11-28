using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.Abstract;

public interface ILoggingRepository : IGenericRepository<RequestLog>
{
    Task AddLogAsync(RequestLog requestLog);
}