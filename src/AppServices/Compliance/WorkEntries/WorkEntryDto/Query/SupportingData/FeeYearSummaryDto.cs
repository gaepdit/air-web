namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query.SupportingData;

public record FeeYearSummaryDto
{
    [Display(Name = "Fee year")]
    public int Year { get; init; }

    [Display(Name = "Invoiced amount")]
    public decimal InvoicedAmount { get; init; }

    [Display(Name = "Amount paid")]
    public decimal AmountPaid { get; init; }

    public decimal Balance { get; init; }

    public string Status { get; init; } = string.Empty;
}
