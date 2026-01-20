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

    // CUSTOMER: Apply Loan
    [HttpPost("apply")]
    public IActionResult ApplyLoan([FromBody] ApplyLoanRequest request)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        if (username == null)
            return Unauthorized();

        var loan = new Loan
        {
            Id = id++,
            Amount = request.Amount,
            TenureMonths = request.TenureMonths,
            InterestRate = 10,
            Emi = EmiCalculator.Calculate(
                request.Amount,
                10,
                request.TenureMonths
            ),
            Username = username,
            Status = LoanStatus.Pending
        };

        loans.Add(loan);

        return Ok(loan);
    }

    // CUSTOMER: View My Loans
    [HttpGet("my")]
    public IActionResult MyLoans()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(loans.Where(l => l.Username == username));
    }

    // BANK: Approve Loan
    [HttpPost("approve/{id}")]
    public IActionResult ApproveLoan(int id)
    {
        var loan = loans.FirstOrDefault(l => l.Id == id);

        if (loan == null)
            return NotFound("Loan not found");

        loan.Status = LoanStatus.Approved;

        return Ok(loan);
    }
}
