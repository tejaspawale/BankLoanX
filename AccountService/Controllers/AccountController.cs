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


        [Authorize]
        [HttpPost("credit")]
        public IActionResult CreditAccount([FromBody] CreditRequest request)
        {
                // var account = accounts.FirstOrDefault(a => a.Id == request.AccountId);
                var account = FakeAccountStore.Accounts.FirstOrDefault(a => a.Id == request.AccountId);

            if (account == null)
                return NotFound("Account not found");

            account.Balance += request.Amount;

            return Ok(account);
        }

        //debit API 
            [Authorize]
            [HttpPost("debit")]
            public IActionResult Debit([FromBody] CreditRequest request)
            {
                var account = FakeAccountStore.Accounts.FirstOrDefault(a => a.Id == request.AccountId);
                if (account == null)
                    return NotFound("Account not found");

                if (account.Balance < request.Amount)
                    return BadRequest("Insufficient balance");

                account.Balance -= request.Amount;
                return Ok(account);
            }

    }
}
