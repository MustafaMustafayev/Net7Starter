using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class ResponseLogRepository : GenericRepository<ResponseLog>, IResponseLogRepository
{
    private readonly DataContext _dataContext;

    public ResponseLogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}