using System.ComponentModel;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Utilities;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.Search;

public enum SortByEnforcement
{
    [Description("DiscoveryDate, Id")] DiscoveryDate
}
public record EnforcementSearchDto
{
    public SortByEnforcement Sort { get; init; } = SortByEnforcement.DiscoveryDate;

    // == Facility ==

    [Display(Name = "Facility AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    // == Staff ==

    [Display(Name = "Staff Responsible")]
    // Guid as string
    public string? ResponsibleStaff { get; init; }

    // == Dates ==

    [Display(Name = "Discovery Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDate { get; init; }

    [Display(Name = "DayZero")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DayZero { get; init; }


    public EnforcementSearchDto TrimAll() => this with
    {
        PartialFacilityId = FacilityId.CleanFacilityId(PartialFacilityId),
    };
}
