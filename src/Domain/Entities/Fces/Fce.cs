using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.Fces;

public class Fce : AuditableSoftDeleteEntity<int>
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Fce() { }

    internal Fce(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties

    public Facility Facility { get; init; } = default!;
    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; init; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; init; }

    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;

    // Properties: Lists
    public List<Comment> Comments { get; } = [];

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}
