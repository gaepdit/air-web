using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using System.ComponentModel;
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
    public ApplicationUser? CurrentReviewer { get; internal set; }
    public DateOnly? ReviewRequestedDate { get; internal set; }

    [UsedImplicitly]
    public ICollection<EnforcementActionReview> Reviews { get; } = [];

    // Status: Approved
    public DateOnly? ApprovedDate { get; internal set; }
    public ApplicationUser? ApprovedBy { get; set; }

    // Status: Issued
    public DateOnly? IssueDate { get; internal set; }
    internal bool IsIssued => IssueDate.HasValue;

    // Status: Closed as unsent
    public DateOnly? ClosedAsUnsentDate { get; internal set; }
    internal bool IsClosedAsUnsent => ClosedAsUnsentDate.HasValue;

    // Data exchange properties
    public bool IsReportable =>
        !IsDeleted &&
        IsIssued &&
        ActionType is EnforcementActionType.AdministrativeOrder
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
    [Description("Letter of Noncompliance")] LetterOfNoncompliance,
    [Description("Notice of Violation")] NoticeOfViolation,
    [Description("No Further Action Letter")] NoFurtherActionLetter,
    [Description("Combined NOV/NFA Letter")] NovNfaLetter,
    [Description("Proposed Consent Order")] ProposedConsentOrder,
    [Description("Consent Order")] ConsentOrder,
    [Description("Order Resolved Letter")] OrderResolvedLetter,
    [Description("Administrative Order")] AdministrativeOrder,
    [Description("Informational Letter")] InformationalLetter,
}

public enum EnforcementActionStatus
{
    [Description("Draft Started")] Draft,
    [Description("Review Requested")] ReviewRequested,
    [Description("Approved")] Approved,
    [Description("Issued")] Issued,
    [Description("Closed As Unsent")] ClosedAsUnsent,
}
