using LoanService.Models;
using LoanService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanService.Controllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public class LoanController : ControllerBase
{
    private static List<Loan> loans = new();
    private static int id = 1;

    [HttpPost("apply")]
    public IActionResult ApplyLoan([FromBody] Loan request)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        if (username == null)
            return Unauthorized();

        request.Id = id++;
        request.Username = username;
        request.InterestRate = 10; // fixed for now
        request.Emi = EmiCalculator.Calculate(
            request.Amount,
            request.InterestRate,
            request.TenureMonths
        );
        request.Status = "Applied";

        loans.Add(request);

        return Ok(request);
    }

    [HttpGet("my")]
    public IActionResult MyLoans()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(loans.Where(l => l.Username == username));
    }
}
