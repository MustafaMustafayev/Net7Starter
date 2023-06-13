using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.DatabaseContext;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class NlogRepository : GenericRepository<Nlog>, INlogRepository
{
    private readonly DataContext _dataContext;

    public NlogRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}