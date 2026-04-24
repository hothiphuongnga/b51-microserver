namespace OrderService.Dtos;

public  class OrderDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

// create order
public class CreateOrderDto
{
    public int UserId { get; set; }

    public decimal Total { get; set; }
    // Mua nhung gi: , masp, soluong
    public List<ProductOrderDto> OrderItems { get; set; } = new List<ProductOrderDto>();
}
public class ProductOrderDto
{
    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}