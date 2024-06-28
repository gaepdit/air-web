using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.Fces;

public class Fce : AuditableSoftDeleteEntity<int>
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Fce() { }

    internal Fce(int? id, FacilityId facilityId, int year)
    {
        if (id is not null) Id = id.Value;
        FacilityId = facilityId;
        Year = year;
    }

    // Properties

    [NotMapped]
    public Facility? Facility { get; internal set; }

    public FacilityId FacilityId { get; init; } = default!;
    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; set; }
    public DateOnly CompletedDate { get; set; }
    public bool OnsiteInspection { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<Comment> Comments { get; } = [];

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}
