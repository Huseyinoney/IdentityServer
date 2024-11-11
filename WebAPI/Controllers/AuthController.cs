using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authManager;
        public AuthController(IAuthService authManager)
        {
            this.authManager = authManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await authManager.LoginAsync(loginDto.Username, loginDto.Password);
            if (response.Length > 0)
            {
                return StatusCode(202,response);
            }
            else
            {
                return StatusCode(400);
            }
        }


        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var response = await authManager.RegisterAsync(registerDto.Username,registerDto.Password);
            
            if(response)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(400);
            }

        }

    }
}
