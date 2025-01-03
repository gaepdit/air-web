﻿using AirWeb.Domain.BaseEntities;
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

    // Staff
    public ApplicationUser? ResponsibleStaff { get; set; }

    // Review process
    public ApplicationUser? CurrentReviewer { get; internal set; }
    public DateOnly? ReviewRequestedDate { get; internal set; }
    public ICollection<EnforcementActionReview> Reviews { get; } = [];
    public bool IsApproved { get; internal set; }
    public DateOnly? ApprovedDate { get; internal set; }
    public ApplicationUser? ApprovedBy { get; set; }

    // Status
    public DateOnly? IssueDate { get; internal set; }
    public bool IsIssued => IssueDate.HasValue;
    public DateOnly? ClosedAsUnsent { get; internal set; }
    public bool IsClosedAsUnsent => ClosedAsUnsent.HasValue;

    // Data flow properties
    public bool IsDataFlowEnabled =>
        !IsDeleted && IsFormalEnforcementAction(ActionType);

    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; init; }

    public static bool IsFormalEnforcementAction(EnforcementActionType type) =>
        type is EnforcementActionType.AdministrativeOrder
            or EnforcementActionType.ConsentOrder
            or EnforcementActionType.NoticeOfViolation
            or EnforcementActionType.NovNfaLetter
            or EnforcementActionType.ProposedConsentOrder;
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum EnforcementActionType
{
    [Description("Administrative Order")] AdministrativeOrder,
    [Description("Administrative Order Resolved")] AoResolvedLetter,
    [Description("Consent Order")] ConsentOrder,
    [Description("Consent Order Resolved")] CoResolvedLetter,
    [Description("Letter")] EnforcementLetter,
    [Description("Letter of Noncompliance")] LetterOfNoncompliance,
    [Description("No Further Action Letter")] NoFurtherAction,
    [Description("Notice of Violation")] NoticeOfViolation,
    [Description("Combined NOV/NFA Letter")] NovNfaLetter,
    [Description("Proposed Consent Order")] ProposedConsentOrder,
}
