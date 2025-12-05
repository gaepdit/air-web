using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using System.Diagnostics.CodeAnalysis;

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

    public ApplicationUser? CurrentReviewer
    {
        get => CurrentOpenReview?.RequestedOf;

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty: This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

    [UsedImplicitly] public DateTime? ReviewRequestedDate
    {
        get => CurrentOpenReview?.RequestedDate;

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty: This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

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
    public bool IsReportableAction { get; internal init; }
    public bool IsReportable => IsReportableAction && IsIssued && !IsDeleted;

    public static bool ActionTypeIsReportable(EnforcementActionType type) => type
        is EnforcementActionType.AdministrativeOrder
        or EnforcementActionType.ConsentOrder
        or EnforcementActionType.NoticeOfViolation
        or EnforcementActionType.NovNfaLetter
        or EnforcementActionType.ProposedConsentOrder;
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
