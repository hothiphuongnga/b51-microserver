// unitofwork
using ProductService.Data;

namespace ProductService.Repositories.Base;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{

    private readonly ProductDbServiceContext _context;
    
    public UnitOfWork(ProductDbServiceContext context)
    {
        _context = context;
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
