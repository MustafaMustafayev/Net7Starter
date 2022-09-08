using Project.DAL.GenericRepositories.Abstract;
using Project.Entity.Entities;

namespace Project.DAL.Abstract;

public interface ILoggingRepository : IGenericRepository<RequestLog>
{
    Task AddLogAsync(RequestLog requestLog);
}