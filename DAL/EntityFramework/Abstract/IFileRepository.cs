using DAL.EntityFramework.GenericRepository;
using File = ENTITIES.Entities.File;

namespace DAL.EntityFramework.Abstract;

public interface IFileRepository : IGenericRepository<File>
{
}