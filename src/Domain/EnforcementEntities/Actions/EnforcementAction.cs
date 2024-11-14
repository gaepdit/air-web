using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public abstract class EnforcementAction : DeletableEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected EnforcementAction() { }

    private protected EnforcementAction(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
    {
        Id = id;
        EnforcementCase = enforcementCase;
        SetCreator(user?.Id);
    }

    // Basic data
    public EnforcementCase EnforcementCase { get; init; } = null!;
    protected EnforcementActionType EnforcementActionType { get; init; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Staff
    public ApplicationUser? ResponsibleStaff { get; set; }

    // Review process
    public ApplicationUser? CurrentOwner { get; internal set; }
    public DateTimeOffset? CurrentOwnerAssignedDate { get; internal set; }
    public ICollection<EnforcementActionReview> Reviews { get; } = [];
    public bool IsApproved { get; internal set; }
    public ICollection<ApplicationUser> ApprovedBy { get; } = [];

    // Status
    public DateOnly? IssueDate { get; internal set; }
    public bool IsIssued => IssueDate.HasValue;
    public DateOnly? ClosedAsUnsent { get; internal set; }
    public bool IsClosedAsUnsent => ClosedAsUnsent.HasValue;

    // Data flow properties
    public bool IsDataFlowEnabled =>
        !IsDeleted &&
        EnforcementActionType
            is EnforcementActionType.AdministrativeOrder
            or EnforcementActionType.ConsentOrder
            or EnforcementActionType.ProposedConsentOrder
            or EnforcementActionType.NoticeOfViolation;

    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(11)]
    public DataExchangeStatus DataExchangeStatus { get; init; } = DataExchangeStatus.NotIncluded;
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum EnforcementActionType
{
    [Description("Letter of Noncompliance")] LetterOfNoncompliance,
    [Description("Notice of Violation")] NoticeOfViolation,
    [Description("No Further Action Letter")] NoFurtherAction,
    [Description("Combined NOV/NFA Letter")] NovNfa,
    [Description("Proposed Consent Order")] ProposedConsentOrder,
    [Description("Consent Order")] ConsentOrder,
    [Description("Consent Order Resolved")] ConsentOrderResolved,
    [Description("Administrative Order")] AdministrativeOrder,
    [Description("Administrative Order Resolved")] AdministrativeOrderResolved,
    [Description("Letter")] Letter,
}
