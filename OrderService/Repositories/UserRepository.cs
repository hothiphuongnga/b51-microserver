using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories.Base;

namespace OrderService.Repositories;

public interface IOrderRepository : IRepositoryBase<Order>
{
    
}

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderDbServiceContext context) : base(context)
    {
        
    }
}