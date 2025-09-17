using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public abstract class EnforcementAction : DeletableEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected EnforcementAction() { }

    private protected EnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
    {
        Id = id;
        CaseFile = caseFile;
        SetCreator(user?.Id);
    }

    // Basic data
    public CaseFile CaseFile { get; init; } = null!;
    public EnforcementActionType ActionType { get; protected init; }

    [StringLength(7000)]
    public string? Notes { get; set; }

    // Status
    public EnforcementActionStatus Status { get; internal set; } = EnforcementActionStatus.Draft;

    // Status: Under review
    public EnforcementActionReview? CurrentOpenReview => Reviews.SingleOrDefault(review => !review.IsCompleted);
    public ApplicationUser? CurrentReviewer => CurrentOpenReview?.RequestedOf;
    [UsedImplicitly] public DateTime? ReviewRequestedDate => CurrentOpenReview?.RequestedDate;
    public ICollection<EnforcementActionReview> Reviews { get; } = [];

    // Status: Approved
    public DateTime? ApprovedDate { get; internal set; }
    public ApplicationUser? ApprovedBy { get; internal set; }

    // Status: Issued
    public DateOnly? IssueDate { get; set; }
    internal bool IsIssued => IssueDate.HasValue;

    // Status: Canceled (closed as unsent)
    public DateTime? CanceledDate { get; internal set; }
    internal bool IsCanceled => CanceledDate.HasValue;

    // Data exchange properties
    public bool IsReportable => WillBeReportable && IsIssued;
    public bool WillBeReportable => !IsDeleted && ActionTypeIsReportable(ActionType);

    public static bool ActionTypeIsReportable(EnforcementActionType type) => type
        is EnforcementActionType.AdministrativeOrder
        or EnforcementActionType.ConsentOrder
        or EnforcementActionType.NoticeOfViolation
        or EnforcementActionType.NovNfaLetter
        or EnforcementActionType.ProposedConsentOrder;

    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}

// The order of these enum values is used by the UI.
public enum EnforcementActionType
{
    [Display(Name = "Letter of Noncompliance")] LetterOfNoncompliance,
    [Display(Name = "Notice of Violation")] NoticeOfViolation,
    [Display(Name = "No Further Action Letter")] NoFurtherActionLetter,
    [Display(Name = "Combined NOV/NFA Letter")] NovNfaLetter,
    [Display(Name = "Proposed Consent Order")] ProposedConsentOrder,
    [Display(Name = "Consent Order")] ConsentOrder,
    [Display(Name = "Administrative Order")] AdministrativeOrder,
    [Display(Name = "Informational Letter")] InformationalLetter,
}

public enum EnforcementActionStatus
{
    [Display(Name = "Draft Started")] Draft,
    [Display(Name = "Review Requested")] ReviewRequested,
    [Display(Name = "Approved")] Approved,
    [Display(Name = "Issued")] Issued,
    [Display(Name = "Canceled (Not Sent)")] Canceled,
}
