using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.Abstract;

public interface IRequestLogRepository : IGenericRepository<RequestLog>
{
    Task AddRequestLogAsync(RequestLog entity);
}