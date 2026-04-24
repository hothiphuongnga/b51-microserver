using ProductService.Data;
using ProductService.Models;
using ProductService.Repositories.Base;

namespace ProductService.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
    
}

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ProductDbServiceContext context) : base(context)
    {
        
    }
}