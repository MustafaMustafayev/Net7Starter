using System.Linq.Expressions;

namespace Project.DAL.GenericRepositories.Abstract;

public interface IGenericRepository<T>
    where T : class, new()
{
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);

    Task<T?> GetAsNoTrackingAsync(Expression<Func<T, bool>> filter);

    IQueryable<T?> GetList(Expression<Func<T, bool>>? filter = null);

    Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null);

    IQueryable<T> GetAsNoTrackingList(Expression<Func<T, bool>>? filter = null);

    Task<T> AddAsync(T entity);

    T Update(T entity);

    void Delete(T entity);

    Task<List<T>> AddRangeAsync(List<T> entity);

    List<T> UpdateRange(List<T> entity);
}