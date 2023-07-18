using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using File = ENTITIES.Entities.File;

namespace DAL.EntityFramework.Concrete;

public class FileRepository : GenericRepository<File>, IFileRepository
{
    private readonly DataContext _dataContext;

    public FileRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}