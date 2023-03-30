using DAL.GenericRepositories.Abstract;
using File = ENTITIES.Entities.File;

namespace DAL.Abstract;

public interface IFileRepository : IGenericRepository<File>
{
}