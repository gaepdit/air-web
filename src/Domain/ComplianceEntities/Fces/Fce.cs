using AirWeb.Domain.Comments;
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

    // FCE Properties

    [MaxLength(9)]
    public string FacilityId { get; [UsedImplicitly] private init; } = string.Empty;

    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; set; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<FceComment> Comments { get; } = [];

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Business Logic

    // The number of calendar years of supporting data covered by an FCE.
    public const int DataPeriod = 1; // One year

    // The number of calendar years of supporting data
    // for fees history and enforcement history.
    public const int ExtendedDataPeriod = 5; // Five years

    // The earliest year for which an FCE exists.
    public const int EarliestFceYear = 2002;

    public static List<int> ValidFceYears
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

public record FceComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private FceComment() { }

    private FceComment(Comment c) : base(c) { }
    public FceComment(Comment c, int fceId) : this(c) => FceId = fceId;
    public int FceId { get; init; }
}
