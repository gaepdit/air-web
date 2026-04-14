using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Core.Entities;
using GaEpd.AppLibrary.Extensions;
using System.Runtime.CompilerServices;

namespace AirWeb.Domain.Compliance.AuditPoints;

public record CaseFileAuditPoint : AuditPoint
{
    [UsedImplicitly] private CaseFileAuditPoint() { } // Used by ORM.
    private CaseFileAuditPoint(AuditPoint auditPoint) : base(auditPoint) { }

    private static CaseFileAuditPoint Create(ApplicationUser? user, string? moreInfo = null,
        [CallerMemberName] string what = "") => new(CreateAuditPoint(what, user, moreInfo));

    private static CaseFileAuditPoint Create(string what, ApplicationUser? user) => new(CreateAuditPoint(what, user));

    private static CaseFileAuditPoint Create(string what, string? moreInfo, ApplicationUser? user) =>
        new(CreateAuditPoint(what, user, moreInfo));

    public int CaseFileId { get; init; }

    internal static CaseFileAuditPoint Added(ApplicationUser? user) => Create(user);
    internal static CaseFileAuditPoint Closed(ApplicationUser? user) => Create(user);
    internal static CaseFileAuditPoint Deleted(ApplicationUser? user) => Create(user);
    internal static CaseFileAuditPoint Edited(ApplicationUser? user) => Create(user);
    internal static CaseFileAuditPoint Reopened(ApplicationUser? user) => Create(user);
    internal static CaseFileAuditPoint Restored(ApplicationUser? user) => Create(user);

    internal static CaseFileAuditPoint ComplianceEventLinked(int eventId, ApplicationUser? user) =>
        Create("Compliance Event Linked", $"Event ID {eventId}", user);

    internal static CaseFileAuditPoint ComplianceEventUnlinked(int eventId, ApplicationUser? user) =>
        Create("Compliance Event Unlinked", $"Event ID {eventId}", user);

    internal static CaseFileAuditPoint PollutantsAndProgramsUpdated(ApplicationUser? user) =>
        Create("Pollutants and Programs Updated", user);

    internal static CaseFileAuditPoint EnforcementActionAdded(EnforcementActionType type, ApplicationUser? user) =>
        Create("Enforcement Action Added", type.GetDisplayName(), user);

    internal static CaseFileAuditPoint EnforcementActionReviewed(EnforcementActionType type, ReviewResult result, ApplicationUser? user) =>
        Create("Enforcement Action Reviewed", $"{type.GetDisplayName()} {result.GetDisplayName()}", user);

    internal static CaseFileAuditPoint EnforcementActionIssued(EnforcementActionType type, ApplicationUser? user) =>
        Create("Enforcement Action Issued", type.GetDisplayName(), user);

    internal static CaseFileAuditPoint EnforcementActionResolved(EnforcementActionType type, ApplicationUser? user) =>
        Create("Enforcement Action Resolved", type.GetDisplayName(), user);

    internal static CaseFileAuditPoint EnforcementActionCanceled(EnforcementActionType type, ApplicationUser? user) =>
        Create("Enforcement Action Canceled", type.GetDisplayName(), user);

    internal static CaseFileAuditPoint EnforcementActionDeleted(EnforcementActionType type, ApplicationUser? user) =>
        Create("Enforcement Action Deleted", type.GetDisplayName(), user);

    internal static CaseFileAuditPoint EnforcementActionOrderExecuted(ApplicationUser? user) =>
        Create("Enforcement Action Order Executed", user);

    internal static CaseFileAuditPoint EnforcementActionOrderAppealed(ApplicationUser? user) =>
        Create("Enforcement Action Order Appealed", user);
}
