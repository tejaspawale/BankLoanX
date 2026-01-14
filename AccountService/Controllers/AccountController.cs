using AccountService.Data;
using AccountService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateAccount()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            var account = new Account
            {
                Id = FakeAccountStore.Accounts.Count + 1,
                Owner = username!,
                Balance = 0
            };

            FakeAccountStore.Accounts.Add(account);

            return Ok(account);
        }

        [HttpGet("my")]
        public IActionResult MyAccounts()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            var accounts = FakeAccountStore.Accounts
                .Where(a => a.Owner == username)
                .ToList();

            return Ok(accounts);
        }
    }
}
