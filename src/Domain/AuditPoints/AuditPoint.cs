using AirWeb.Domain.Identity;
using System.Runtime.CompilerServices;

namespace AirWeb.Domain.AuditPoints;

public record AuditPoint : IEntity
{
    protected static AuditPoint CreateAuditPoint(string what, ApplicationUser? user, string? moreInfo) => new()
    {
        Id = Guid.NewGuid(),
        When = DateTimeOffset.Now,
        Who = user,
        What = what,
        MoreInfo = moreInfo,
    };

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private init; }

    public DateTimeOffset When { get; private init; }
    public ApplicationUser? Who { get; private init; }

    [StringLength(10)]
    public string What { get; private init; } = null!;

    [StringLength(500)]
    public string? MoreInfo { get; init; }
}

public record WorkEntryAuditPoint : AuditPoint
{
    [UsedImplicitly] protected WorkEntryAuditPoint() { } // Used by ORM.
    private WorkEntryAuditPoint(AuditPoint auditPoint, int workEntryId) : base(auditPoint) => WorkEntryId = workEntryId;

    private static WorkEntryAuditPoint Create(int workEntryId, ApplicationUser? user, string? moreInfo = null,
        [CallerMemberName] string what = "") =>
        new(CreateAuditPoint(what, user, moreInfo), workEntryId);

    public int WorkEntryId { get; init; }

    public static WorkEntryAuditPoint Add(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);

    internal static WorkEntryAuditPoint Close(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);

    public static WorkEntryAuditPoint Delete(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);

    public static WorkEntryAuditPoint Edit(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);

    public static WorkEntryAuditPoint Reopen(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);

    public static WorkEntryAuditPoint Restore(int workEntryId, ApplicationUser? user, string? moreInfo = null) =>
        Create(workEntryId, user, moreInfo);
}
