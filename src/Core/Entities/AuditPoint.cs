namespace AirWeb.Core.Entities;

public record AuditPoint : IEntity
{
    protected static AuditPoint CreateAuditPoint(string what, ApplicationUser? user, string? moreInfo = null) => new()
    {
        Id = Guid.NewGuid(),
        When = DateTimeOffset.Now,
        Who = user,
        What = what,
        MoreInfo = moreInfo,
    };

    public Guid Id { get; private init; }

    [StringLength(100)]
    public string What { get; private init; } = null!;

    public ApplicationUser? Who { get; private init; }
    public DateTimeOffset When { get; private init; }

    [StringLength(500)]
    public string? MoreInfo { get; init; }
}
