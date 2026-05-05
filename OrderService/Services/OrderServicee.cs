using System.Text.Json;
using AutoMapper;
using Confluent.Kafka;
using OrderService.Dtos;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Repositories.Base;
using OrderService.Services.Base;

namespace OrderService.Services;

public interface IOrderServicee : IServiceBase<Order, OrderDto>
{
    Task<ResponseEntity> CreateOrderAsync(CreateOrderDto order);

}

public class OrderServicee : ServiceBase<Order, OrderDto>, IOrderServicee
{

    private readonly IOrderRepository _OrderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _factory;
    private readonly IKafkaProducer _kafkaProducer;

    public OrderServicee(IOrderRepository OrderRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpClientFactory factory, IKafkaProducer kafkaProducer) : base(unitOfWork, mapper, OrderRepository)
    {
        _OrderRepository = OrderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _factory = factory;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<ResponseEntity> CreateOrderAsync(CreateOrderDto order)
    {
        var orderEntity = new Order()
        {
            Id = 0,
            UserId = order.UserId,
            Total = order.Total,
            CreatedAt = DateTime.Now,
        };

        // thêm orderitem 
        // [có nhieu ỏderitem -> chay vong lap]
        foreach (var item in order.OrderItems)
        {
            var od = new OrderItem()
            {
                Id = 0,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
            };
            orderEntity.OrderItems.Add(od);
        }

        await _OrderRepository.AddAsync(orderEntity);

        await _unitOfWork.SaveChangesAsync();
        // update tồn kho
        //[CODE 4/5/2026]
        // gửi lên kafka
        var message = new Message<string, string>
        {
            Key = orderEntity.Id.ToString(),
            Value = JsonSerializer.Serialize(order.OrderItems)
        };

        await _kafkaProducer.ProducerAsyns("order-created-topic", message);

        
        






        // gọi qua product service để update tồn kho
        // dat nhieu sp=> chay vong lap

        // api update ton kho ; : PUT: /api/product/update-stock/{id}?quantity={soluong}
        // var client = _factory.CreateClient("ProductService");
        // foreach(var item in order.OrderItems)
        // {
        //     var response = await client.PutAsync($"api/product/update-stock/{item.ProductId}?quantity={item.Quantity}", null);
        //     // xử lý response nếu cần thiết (ví dụ: kiểm tra xem có thành công hay không)
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         // Xử lý lỗi (ví dụ: log lỗi, trả về lỗi cho client, v.v.)
        //         return ResponseEntity.Fail($"Failed to update stock for product ID {item.ProductId}");
        //     }

        // }
        return ResponseEntity.Ok(_map.Map<OrderDto>(orderEntity));
    }
}