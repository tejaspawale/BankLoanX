namespace LoanService.Models;

public class ApplyLoanRequest
{
    public decimal Amount { get; set; }
    public int TenureMonths { get; set; }
}
