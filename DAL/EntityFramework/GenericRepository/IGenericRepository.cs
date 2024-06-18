using System.Linq.Expressions;

namespace DAL.EntityFramework.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    IQueryable<T> GetList(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false);
    Task<T?> GetAsNoTrackingAsync(Expression<Func<T, bool>> filter);
    Task<T?> GetAsNoTrackingWithIdentityResolutionAsync(Expression<Func<T, bool>> filter);
    Task<int> CountAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    Task<bool> AllAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    Task<T?> FindAsync(int id);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    Task<T?> SingleAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    Task<T?> FirstAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
    IQueryable<T> GetAsNoTrackingList(Expression<Func<T, bool>>? filter = null);
    IQueryable<T> GetAsNoTrackingWithIdentityResolutionListAsync(Expression<Func<T, bool>>? filter = null);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SoftDeleteAsync(T entity);
    Task<List<T>> AddRangeAsync(List<T> entity);
    Task<List<T>> UpdateRangeAsync(List<T> entity);
}