namespace ProductService.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using UserService.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _service.GetAllAsync();
        }
        [HttpPut("update-stock/{id}")]
        public async Task<IActionResult> Put(int id, int quantity)
        {
            return await _service.UpdateStockAsync(id, quantity);
        }

    }
}