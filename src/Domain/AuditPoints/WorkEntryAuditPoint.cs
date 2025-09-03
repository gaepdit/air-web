using AirWeb.Domain.Identity;
using System.Runtime.CompilerServices;

namespace AirWeb.Domain.AuditPoints;

public record WorkEntryAuditPoint : AuditPoint
{
    [UsedImplicitly] private WorkEntryAuditPoint() { } // Used by ORM.
    private WorkEntryAuditPoint(AuditPoint auditPoint) : base(auditPoint) { }

    private static WorkEntryAuditPoint Create(ApplicationUser? user, [CallerMemberName] string what = "") =>
        new(CreateAuditPoint(what, user));

    public int WorkEntryId { get; init; }

    internal static WorkEntryAuditPoint Added(ApplicationUser? user) => Create(user);
    internal static WorkEntryAuditPoint Closed(ApplicationUser? user) => Create(user);
    internal static WorkEntryAuditPoint Deleted(ApplicationUser? user) => Create(user);
    internal static WorkEntryAuditPoint Edited(ApplicationUser? user) => Create(user);
    internal static WorkEntryAuditPoint Reopened(ApplicationUser? user) => Create(user);
    internal static WorkEntryAuditPoint Restored(ApplicationUser? user) => Create(user);
}
