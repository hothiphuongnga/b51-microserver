
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<CreateOrderDto, Order>();
        
    }
}