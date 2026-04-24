namespace ProductService.Repositories.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    Task DeleteAsync(int id);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null);
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly ProductDbServiceContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(ProductDbServiceContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }
        
        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)  => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

}
