using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IRequestLogRepository : IGenericRepository<RequestLog>
{
    Task AddRequestLogAsync(RequestLog entity);
}