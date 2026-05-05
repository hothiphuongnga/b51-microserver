namespace ProductService.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using ProductService.Services;

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

        // viết  nhanh api upload hình ảnh


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            return await _service.UploadImageAsync(file);
        }

    }
}