
using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class ITMIMRepository : GenericRepository<ITMIM>, IITMIMRepository
{
    private readonly DataContext _dataContext;

    public ITMIMRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}
