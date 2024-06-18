using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class RequestLogRepository : GenericRepository<RequestLog>, IRequestLogRepository
{
    private readonly DataContext _dataContext;

    public RequestLogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    //public async Task AddRequestLogAsync(RequestLog entity)
    //{
    //    await _dataContext.RequestLogs.AddAsync(entity);
    //}
}