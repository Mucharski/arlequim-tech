using System.Linq.Expressions;
using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ArlequimTech.Core.BaseClasses;

public class BaseRepository<T> : IBaseRepository<T> where T : DatabaseEntity, new()
{
    protected DbSet<T> _dbSet;
    private Context _context;

    public BaseRepository(Context context)
    {
        _dbSet = context.Set<T>();
        _context = context;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        if (expression is not null)
            entity = entity.Where(expression);

        return await entity.FirstOrDefaultAsync();
    }

    public async Task<TResult> GetWithSelectAsync<TResult>(Expression<Func<T, bool>> expression,
        Expression<Func<T, TResult>> selector)
    {
        return await _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution()
            .Where(expression)
            .Select(selector)
            .FirstOrDefaultAsync();
    }

    public T Get(Expression<Func<T, bool>> expression)
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        if (expression is not null)
            entity = entity.Where(expression);

        return entity.FirstOrDefault();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        if (expression is not null)
            entity = entity.Where(expression);

        foreach (var include in includes)
            entity = entity.Include(include);

        return await entity.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        return await entity.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        foreach (var include in includes)
            entity = entity.Include(include);

        return await entity.ToListAsync();
    }

    public async Task<List<TResult>> GetAllWithSelectAsync<TResult>(Expression<Func<T, TResult>> selector)
    {
        return await _dbSet
            .AsNoTrackingWithIdentityResolution()
            .Select(selector)
            .ToListAsync();
    }

    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet
            .Where(predicate)
            .AsNoTrackingWithIdentityResolution()
            .ToList();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        var entity = _dbSet
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        if (predicate is not null)
            entity = entity.Where(predicate);

        foreach (var include in includes)
            entity = entity.Include(include);

        return await entity.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public async Task AddRangeAsync(T[] entity)
    {
        await _dbSet.AddRangeAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<T> entity)
    {
        await _dbSet.AddRangeAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpsertAsync(T entity, Expression<Func<T, bool>> predicate)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(predicate);

        if (existingEntity == null)
            await _dbSet.AddAsync(entity);
        else
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        await Task.FromResult(_dbSet.Update(entity));
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public async Task Remove(Guid id)
    {
        _dbSet.Remove(new T { Id = id });
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Expression<Func<T, bool>> predicate)
    {
        await _context.Set<T>().Where(predicate).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
        _context.SaveChanges();
    }
}