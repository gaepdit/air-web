using AirWeb.Domain.Identity;

namespace AirWeb.Domain.AuditPoints;

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

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private init; }

    [StringLength(10)]
    public string What { get; private init; } = null!;

    public ApplicationUser? Who { get; private init; }
    public DateTimeOffset When { get; private init; }

    [StringLength(500)]
    public string? MoreInfo { get; init; }
}
