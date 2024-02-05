using FoodOrderSystemAPI.DTOs.Auth;
using FoodOrderSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderSystemAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterDTO user)
        {
            var message = await _service.Register(user);
            return message;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO user)
        {
            var message = await _service.Login(user);
            return message;
        }

    }
}
