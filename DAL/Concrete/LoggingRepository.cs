using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities.Logging;

namespace DAL.Concrete;

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