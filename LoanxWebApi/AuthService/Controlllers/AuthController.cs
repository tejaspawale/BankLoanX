using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            FakeUserStore.Users.Add(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var existingUser = FakeUserStore.Users.FirstOrDefault(u =>
                u.Username == user.Username &&
                u.Password == user.Password);

            if (existingUser == null)
                return Unauthorized("Invalid username or password");

            return Ok("Login successful");
        }
    }
}
