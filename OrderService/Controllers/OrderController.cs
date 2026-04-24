namespace OrderService.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using OrderService.Dtos;
    using OrderService.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderServicee _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            return await _service.GetAllAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto order)
        {
            // goij serrvice tuong ung
            return await _service.CreateOrderAsync(order);
        }
    }
}