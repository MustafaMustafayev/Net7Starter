using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class ResponseLogRepository : GenericRepository<ResponseLog>, IResponseLogRepository
{
    private readonly DataContext _dataContext;

    public ResponseLogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}