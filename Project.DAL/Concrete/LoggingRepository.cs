using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete;

public class LoggingRepository : GenericRepository<RequestLog>, ILoggingRepository
{
    private readonly DataContext _dataContext;

    public LoggingRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddLogAsync(RequestLog requestLog)
    {
        await _dataContext.RequestLogs.AddAsync(requestLog);
    }
}