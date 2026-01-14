using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        // 🔒 Protected endpoint
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureData()
        {
            return Ok("You are authorized. JWT is valid.");
        }

        // 🔓 Public endpoint
        [HttpGet("public")]
        public IActionResult PublicData()
        {
            return Ok("This is public. No token needed.");
        }
    }
}
