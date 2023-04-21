using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class RequestLogRepository : GenericRepository<RequestLog>, IRequestLogRepository
{
    private readonly DataContext _dataContext;

    public RequestLogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}