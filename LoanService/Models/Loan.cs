namespace LoanService.Models;

public class Loan
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public decimal Amount { get; set; }
    public int TenureMonths { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Emi { get; set; }
    public string Status { get; set; } = "Applied";
}
