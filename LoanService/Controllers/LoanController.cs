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

[HttpPost("disburse/{id}")]
public async Task<IActionResult> DisburseLoan(int id)
{
    var loan = loans.FirstOrDefault(l => l.Id == id);

    if (loan == null)
        return NotFound("Loan not found");

    if (loan.Status != LoanStatus.Approved)
        return BadRequest("Loan not approved");

    var client = new HttpClient();

    var token = Request.Headers["Authorization"].ToString();

    client.DefaultRequestHeaders.Add("Authorization", token);

    var response = await client.PostAsJsonAsync(
        "http://localhost:5028/api/accounts/credit",
        new
        {
            AccountId = 1,        // temporary (we’ll improve this)
            Amount = loan.Amount
        });

    if (!response.IsSuccessStatusCode)
        return BadRequest("Account credit failed");

    loan.Status = LoanStatus.Disbursed;

    return Ok(loan);
}


}
