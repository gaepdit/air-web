using AirWeb.Domain.Identity;
using System.Runtime.CompilerServices;

namespace AirWeb.Domain.AuditPoints;

public record FceAuditPoint : AuditPoint
{
    [UsedImplicitly] private FceAuditPoint() { } // Used by ORM.
    private FceAuditPoint(AuditPoint auditPoint) : base(auditPoint) { }

    private static FceAuditPoint Create(ApplicationUser? user, [CallerMemberName] string what = "") =>
        new(CreateAuditPoint(what, user));

    public int FceId { get; init; }

    internal static FceAuditPoint Added(ApplicationUser? user) => Create(user);
    internal static FceAuditPoint Deleted(ApplicationUser? user) => Create(user);
    internal static FceAuditPoint Edited(ApplicationUser? user) => Create(user);
    internal static FceAuditPoint Restored(ApplicationUser? user) => Create(user);
}
