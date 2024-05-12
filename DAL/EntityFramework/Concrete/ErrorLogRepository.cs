using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class ErrorLogRepository : GenericRepository<ErrorLog>, IErrorLogRepository
{
    private readonly DataContext _dataContext;

    public ErrorLogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}