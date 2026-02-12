using AirWeb.Core.BaseEntities;
using AirWeb.Core.Entities;
using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Comments;
using AirWeb.Domain.DataExchange;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public class Fce : DeletableEntity<int>, INotes, IDataExchangeAction, IComments<FceComment>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private Fce() { }

    internal Fce(int? id, FacilityId facilityId, int year, ApplicationUser? user = null)
    {
        if (id is not null) Id = id.Value;
        FacilityId = facilityId;
        Year = year;

        //  Set Completed Date to today for FCEs within the current fiscal year
        // or September 30 for FCEs created after the selected fiscal year has ended.
        var fiscalEndDate = new DateOnly(year, month: 9, day: 30);
        var today = DateOnly.FromDateTime(DateTime.Today);
        CompletedDate = today > fiscalEndDate ? fiscalEndDate : today;

        SetCreator(user?.Id);
    }

    // FCE properties

    [StringLength(9)]
    public string FacilityId { get; init; } = null!;

    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; set; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; set; }

    [StringLength(7000)]
    public string? Notes { get; set; }

    // Comments
    public List<FceComment> Comments { get; } = [];

    // Audit Points
    public List<FceAuditPoint> AuditPoints { get; } = [];

    // Business Logic

    // The number of calendar years of supporting data covered by an FCE.
    public const int DataPeriod = 1; // One year

    // The number of calendar years of supporting data
    // for fees history and enforcement history.
    public const int ExtendedDataPeriod = 5; // Five years

    // The earliest year for which an FCE exists.
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

    [JsonIgnore]
    public ushort? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; set; }

    [JsonIgnore]
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
