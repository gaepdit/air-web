using AirWeb.Domain.Identity;

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

        //  Set Completed Date to today for FCEs within the current fiscal year
        // or September 30 for FCEs created after the selected fiscal year has ended.
        var fiscalEndDate = new DateOnly(year, 9, 30);
        var today = DateOnly.FromDateTime(DateTime.Today);
        CompletedDate = today > fiscalEndDate ? fiscalEndDate : today;
    }

    // Facility properties
    [StringLength(9)]
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

    // FCE properties
    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; set; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Comments
    public List<FceComment> Comments { get; } = [];

    // Deletion properties
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Business Logic
    public const int EarliestFceYear = 2002;

    public static ICollection<int> ValidFceYears
    {
        get
        {
            var currentYear = DateTime.Today.Year;
            var yearList = new List<int> { currentYear };

            if (DateTime.Today.Month < 10)
                yearList.Add(currentYear - 1);
            else
                yearList.Insert(0, currentYear + 1);

            return yearList;
        }
    }
}
