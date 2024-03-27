using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Concrete;

public class TestRepository : GenericRepository<Test>, ITestRepository
{
    private readonly DataContext _dataContext;

    public TestRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}
