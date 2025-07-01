namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record FeeYearSummaryDto
{
    [Display(Name = "Fee Year")]
    public int Year { get; init; }

    [Display(Name = "Invoiced Amount")]
    public decimal InvoicedAmount { get; init; }

    [Display(Name = "Amount Paid")]
    public decimal AmountPaid { get; init; }

    public decimal Balance { get; init; }

    public string Status { get; init; } = null!;
}
