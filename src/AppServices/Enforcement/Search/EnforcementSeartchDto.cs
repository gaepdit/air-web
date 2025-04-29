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
    [Display(Name = "Facility AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    public SortByEnforcement Sort { get; init; } = SortByEnforcement.DiscoveryDate;

    // == Text == 
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
    // == Dates ==
    [Display(Name = "Discovery Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDate { get; init; }
}
