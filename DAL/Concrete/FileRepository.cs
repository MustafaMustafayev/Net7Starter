using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using File = ENTITIES.Entities.File;

namespace DAL.Concrete;

public class FileRepository : GenericRepository<File>, IFileRepository
{
    private readonly DataContext _dataContext;

    public FileRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}