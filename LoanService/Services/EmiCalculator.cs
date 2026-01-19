namespace LoanService.Services;

public static class EmiCalculator
{
    public static decimal Calculate(decimal principal, decimal annualRate, int months)
    {
        var r = annualRate / 12 / 100;
        var n = months;

        if (r == 0)
            return principal / n;

        var factor = (decimal)Math.Pow((double)(1 + r), n);
        return principal * r * factor / (factor - 1);
    }
}
