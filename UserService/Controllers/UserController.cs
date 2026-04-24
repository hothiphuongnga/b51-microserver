namespace UserService.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using UserService.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServicee _userService;
        public UserController(IUserServicee userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ress = await _userService.GetAllAsync();
            return ress;
        }
    }
}