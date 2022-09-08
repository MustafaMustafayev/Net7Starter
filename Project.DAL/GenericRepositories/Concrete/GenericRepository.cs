using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Abstract;

namespace Project.DAL.GenericRepositories.Concrete;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, new()
{
    private readonly DataContext _ctx;

    public GenericRepository(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var newEntity = _ctx.CreateProxy<TEntity>();
        _ctx.Entry(newEntity).CurrentValues.SetValues(entity);
        _ctx.Entry(entity).State = EntityState.Detached;
        await _ctx.AddAsync(newEntity);
        return newEntity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
    {
        await _ctx.AddRangeAsync(entity);
        return entity;
    }

    public void Delete(TEntity entity)
    {
        _ctx.Remove(entity);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _ctx.Set<TEntity>().FirstOrDefaultAsync(filter);
    }

    public async Task<TEntity> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _ctx.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
    }

    public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
    {
        return filter == null ? _ctx.Set<TEntity>() : _ctx.Set<TEntity>().Where(filter);
    }

    public IQueryable<TEntity> GetAsNoTrackingList(Expression<Func<TEntity, bool>> filter = null)
    {
        return (filter == null ? _ctx.Set<TEntity>().AsNoTracking() : _ctx.Set<TEntity>().Where(filter)).AsNoTracking();
    }

    public TEntity Update(TEntity entity)
    {
        var updatedEntity = _ctx.CreateProxy<TEntity>();
        _ctx.Entry(updatedEntity).CurrentValues.SetValues(entity);
        _ctx.Entry(entity).State = EntityState.Detached;
        _ctx.Update(updatedEntity);
        return updatedEntity;
    }

    public List<TEntity> UpdateRange(List<TEntity> entity)
    {
        _ctx.UpdateRange(entity);
        return entity;
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        return filter == null
            ? await _ctx.Set<TEntity>().ToListAsync()
            : await _ctx.Set<TEntity>().Where(filter).ToListAsync();
    }
}