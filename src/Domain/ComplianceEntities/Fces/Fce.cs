using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public class Fce : AuditableSoftDeleteEntity<int>, IComplianceEntity
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

    // Facility Properties

    [MaxLength(9)]
    public string FacilityId { get; private set; } = string.Empty;

    private Facility _facility = default!;

    [NotMapped]
    public Facility Facility
    {
        get => _facility;
        set
        {
            _facility = value;
            FacilityId = value.Id;
        }
    }

    // FCE Properties

    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; set; }
    public DateOnly CompletedDate { get; set; }
    public bool OnsiteInspection { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<FceComment> Comments { get; } = [];

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}

public record FceComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private FceComment() { }

    private FceComment(Comment c) : base(c) { }
    public FceComment(Comment c, int fceId) : this(c) => FceId = fceId;
    public int FceId { get; init; }
}
