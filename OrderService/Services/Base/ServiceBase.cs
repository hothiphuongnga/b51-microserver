using System.Linq.Expressions;
using AutoMapper;
using OrderService.Repositories.Base;

namespace OrderService.Services.Base;

public interface IServiceBase<TEntity, TDto> where TEntity : class
{
    Task<ResponseEntity> GetAllAsync();
    Task<ResponseEntity> GetByIdAsync(int id);
    Task<ResponseEntity> AddAsync(TDto entity);
    Task<ResponseEntity> UpdateAsync(TDto entity);
    Task<ResponseEntity> DeleteAsync(int id);
    Task<ResponseEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<ResponseEntity> WhereAsync(Expression<Func<TEntity, bool>> predicate);
}



public class ServiceBase<TEntity, TDto> : IServiceBase<TEntity, TDto> where TEntity : class
{
    protected readonly IUnitOfWork _uow;
    protected readonly IRepositoryBase<TEntity> _repositoryBase;
    protected readonly IMapper _map;

    public ServiceBase(IUnitOfWork uow, IMapper map, IRepositoryBase<TEntity> repositoryBase)
    {
        _uow = uow;
        _map = map;
        _repositoryBase = repositoryBase;
    }

    public virtual async Task<ResponseEntity> GetAllAsync()
    {
        var res = await _repositoryBase.GetAllAsync();
        return ResponseEntity.Ok(_map.Map<IEnumerable<TDto>>(res));
    }

    public virtual async Task<ResponseEntity> GetByIdAsync(int id)
    {
        var res = await _repositoryBase.GetByIdAsync(id);
        return ResponseEntity.Ok(_map.Map<TDto?>(res));
    }

    public virtual async Task<ResponseEntity> AddAsync(TDto entity)
    {
        var mapped = _map.Map<TEntity>(entity);
        await _repositoryBase.AddAsync(mapped);
        await _uow.SaveChangesAsync();
        return ResponseEntity.Ok(_map.Map<TDto>(mapped));
    }

    public virtual async Task<ResponseEntity> UpdateAsync(TDto entity)
    {
        _repositoryBase.Update(_map.Map<TEntity>(entity));
        await _uow.SaveChangesAsync();
        return ResponseEntity.Ok(_map.Map<TDto>(entity));
    }

    public virtual async Task<ResponseEntity> DeleteAsync(int id)
    {
        await _repositoryBase.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return ResponseEntity.Ok(id);
    }

    public async Task<ResponseEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var res = await _repositoryBase.SingleOrDefaultAsync(predicate);
        return ResponseEntity.Ok(_map.Map<TDto?>(res));
    }

    public async Task<ResponseEntity> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var res = await _repositoryBase.WhereAsync(predicate);
        return ResponseEntity.Ok(_map.Map<IEnumerable<TDto>>(res));
    }
}
