using AirWeb.Core.Entities;
using System.Runtime.CompilerServices;

namespace AirWeb.Domain.AuditPoints;

public record ComplianceWorkAuditPoint : AuditPoint
{
    [UsedImplicitly] private ComplianceWorkAuditPoint() { } // Used by ORM.
    private ComplianceWorkAuditPoint(AuditPoint auditPoint) : base(auditPoint) { }

    private static ComplianceWorkAuditPoint Create(ApplicationUser? user, [CallerMemberName] string what = "") =>
        new(CreateAuditPoint(what, user));

    public int ComplianceWorkId { get; init; }

    internal static ComplianceWorkAuditPoint Added(ApplicationUser? user) => Create(user);
    internal static ComplianceWorkAuditPoint Closed(ApplicationUser? user) => Create(user);
    internal static ComplianceWorkAuditPoint Deleted(ApplicationUser? user) => Create(user);
    internal static ComplianceWorkAuditPoint Edited(ApplicationUser? user) => Create(user);
    internal static ComplianceWorkAuditPoint Reopened(ApplicationUser? user) => Create(user);
    internal static ComplianceWorkAuditPoint Restored(ApplicationUser? user) => Create(user);
}
