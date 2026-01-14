using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
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

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, existingUser.Username)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );

    return Ok(new
    {
        token = new JwtSecurityTokenHandler().WriteToken(token)
    });
}

    }
}
