using System;
using System.Collections.Generic;

namespace OrderService.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
