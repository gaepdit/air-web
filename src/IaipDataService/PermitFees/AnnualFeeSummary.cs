using System.ComponentModel.DataAnnotations;

namespace IaipDataService.PermitFees;

public record AnnualFeeSummary
{
    [Display(Name = "Fee Year")]
    public int FeeYear { get; init; }

    [Display(Name = "Invoiced Amount")]
    public decimal InvoicedAmount { get; init; }

    [Display(Name = "Amount Paid")]
    public decimal PaidAmount { get; init; }

    public decimal Balance { get; init; }
    public AnnualFeeStatus Status { get; init; }
}

// Values are stored in `airbranch.dbo.FS_ADMIN.NUMCURRENTSTATUS`.
// Display name comes from `airbranch.dbo.FSLK_ADMIN_STATUS`.
public enum AnnualFeeStatus
{
    [Display(Name = "Unknown")]
    Unknown = 0,

    [Display(Name = "Added to the Annual Fee System")]
    AddedToSystem = 1,

    [Display(Name = "Added to the Annual Fee Mailout")]
    AddedToMailout = 2,

    [Display(Name = "Facility Enrolled in the Fee System")]
    Enrolled = 3,

    [Display(Name = "Facility Mailed a Fee Letter")]
    MailoutSent = 4,

    [Display(Name = "GECO User has updated the Fee Contact")]
    GecoContactUpdated = 5,

    [Display(Name = "GECO User has updated the Fee Calculations")]
    GecoFeeCalcUpdated = 6,

    [Display(Name = "GECO User Has updated the Signature Page")]
    GecoSignaturePageUpdated = 7,

    [Display(Name = "GECO User has reported for the Fee Year")]
    GecoFeesReported = 8,

    [Display(Name = "Partial Payment")]
    PartialPayment = 9,

    [Display(Name = "Paid in Full")]
    PaidInFull = 10,

    [Display(Name = "Out of Balance")]
    OutOfBalance = 11,

    [Display(Name = "Collections Ceased due to Audit")]
    CollectionsCeased = 12,
}
