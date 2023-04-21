using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class NlogRepository : GenericRepository<Nlog>, INlogRepository
{
    private readonly DataContext _dataContext;

    public NlogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}