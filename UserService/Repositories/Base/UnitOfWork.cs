// unitofwork
using UserService.Data;

namespace UserService.Repositories.Base;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{

    private readonly UserDbServiceContext _context;
    
    public UnitOfWork(UserDbServiceContext context)
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
