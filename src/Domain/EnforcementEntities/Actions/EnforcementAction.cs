using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public abstract class EnforcementAction : ClosableEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected EnforcementAction() { }

    private protected EnforcementAction(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
    {
        Id = id;
        EnforcementCase = enforcementCase;
    }

    // Basic data
    public EnforcementCase EnforcementCase { get; init; } = null!;
    public EnforcementActionType EnforcementActionType { get; internal init; }

    public bool IsEpaEnforcementAction =>
        EnforcementActionType != EnforcementActionType.NoFurtherAction &&
        EnforcementActionType != EnforcementActionType.LetterOfNoncompliance;

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Staff
    public ApplicationUser? ResponsibleStaff { get; set; }

    // Review process
    public ApplicationUser? CurrentOwner { get; internal set; }
    public DateTimeOffset? CurrentOwnerAssignedDate { get; internal set; }
    public ICollection<EnforcementActionReview> Reviews { get; } = [];
    public ICollection<ApplicationUser> ApprovedBy { get; } = [];

    // Status
    public bool Approved { get; internal set; }
    public bool Issued { get; internal set; }
    public DateOnly? IssueDate { get; internal set; }

    // Data flow properties
    public short? ActionNumber { get; set; }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum EnforcementActionType
{
    [Description("Letter of Noncompliance")] LetterOfNoncompliance,
    [Description("Notice of Violation")] NoticeOfViolation,
    [Description("No Further Action Letter")] NoFurtherAction,
    [Description("Proposed Consent Order")] ProposedConsentOrder,
    [Description("Consent Order")] ConsentOrder,
    [Description("Administrative Order")] AdministrativeOrder,
}
