using DAL.EntityFramework.Context;
using ENTITIES.Entities.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EntityFramework.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
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

    public void SoftDelete(TEntity entity)
    {
        var property = entity.GetType().GetProperty(nameof(Auditable.IsDeleted));

        if (property is null)
            throw new ArgumentException(
                @$"The property with type: {entity.GetType()} can not be SoftDeleted, 
                        because it doesn't contains {nameof(Auditable.IsDeleted)} property, 
                        and did not implemented {typeof(Auditable)}.");

        if (((bool?)property.GetValue(entity)!).Value)
            throw new Exception("This entity was already deleted.");

        property.SetValue(entity, true);

        var updatedEntity = _ctx.CreateProxy<TEntity>();

        _ctx.Entry(updatedEntity).CurrentValues.SetValues(entity);
        _ctx.Entry(entity).State = EntityState.Detached;
        _ctx.Update(updatedEntity);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().FirstOrDefaultAsync(filter)
            : await _ctx.Set<TEntity>().FirstOrDefaultAsync(filter);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
        bool ignoreQueryFilters = false)
    {
        return filter is null
            ? ignoreQueryFilters
                ? await _ctx.Set<TEntity>().IgnoreQueryFilters().ToListAsync()
                : await _ctx.Set<TEntity>().ToListAsync()
            : ignoreQueryFilters
                ? await _ctx.Set<TEntity>().Where(filter).IgnoreQueryFilters().ToListAsync()
                : await _ctx.Set<TEntity>().Where(filter).ToListAsync();
    }

    public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>>? filter = null, bool ignoreQueryFilters = false)
    {
        return filter is null
            ? ignoreQueryFilters
                ? _ctx.Set<TEntity>().IgnoreQueryFilters()
                : _ctx.Set<TEntity>()
            : ignoreQueryFilters
                ? _ctx.Set<TEntity>().Where(filter).IgnoreQueryFilters()
                : _ctx.Set<TEntity>().Where(filter);
    }

    public async Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _ctx.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(filter);
    }

    public IQueryable<TEntity> GetAsNoTrackingList(Expression<Func<TEntity, bool>>? filter = null)
    {
        return (filter is null
            ? _ctx.Set<TEntity>().AsNoTracking()
            : _ctx.Set<TEntity>().Where(filter)).AsNoTracking();
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

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().CountAsync(filter)
            : await _ctx.Set<TEntity>().CountAsync(filter);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().AnyAsync(filter)
            : await _ctx.Set<TEntity>().AnyAsync(filter);
    }

    public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().AllAsync(filter)
            : await _ctx.Set<TEntity>().AllAsync(filter);
    }

    public async Task<TEntity?> FindAsync(int id)
    {
        return await _ctx.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
        bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().SingleOrDefaultAsync(filter)
            : await _ctx.Set<TEntity>().SingleOrDefaultAsync(filter);
    }

    public async Task<TEntity?> SingleAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().SingleAsync(filter)
            : await _ctx.Set<TEntity>().SingleAsync(filter);
    }

    public async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await _ctx.Set<TEntity>().IgnoreQueryFilters().FirstAsync(filter)
            : await _ctx.Set<TEntity>().FirstAsync(filter);
    }

    // public async Task<IQueryable> GetListAsync(Expression<Func<TEntity, bool>>? filter = null, bool ignoreQueryFilters = false)
    // {
    //     return filter is null
    //         ? ignoreQueryFilters
    //             ? await _ctx.Set<TEntity>().IgnoreQueryFilters().ToListAsync()
    //             : await _ctx.Set<TEntity>().ToListAsync()
    //         : ignoreQueryFilters
    //             ? await _ctx.Set<TEntity>().Where(filter).IgnoreQueryFilters().ToListAsync()
    //             : await _ctx.Set<TEntity>().Where(filter).ToListAsync();
    // }
}