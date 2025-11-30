using System.Linq.Expressions;

namespace ArlequimTech.Core.BaseClasses.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetAsync(Expression<Func<T, bool>> expression);
    Task<TResult> GetWithSelectAsync<TResult>(Expression<Func<T, bool>> expression,
        Expression<Func<T, TResult>> selector);
    T Get(Expression<Func<T, bool>> expression);
    Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    //Expressions Filters
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    void Add(T entity);
    Task AddRangeAsync(T[] entity);
    Task AddRangeAsync(List<T> entity);
    Task UpdateAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task Remove(Guid id);
    Task RemoveAsync(Expression<Func<T, bool>> predicate);
    void RemoveRange(IEnumerable<T> entity);
}