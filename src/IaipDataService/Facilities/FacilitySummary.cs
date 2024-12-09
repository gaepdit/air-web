using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Facilities;

// FUTURE: Is this class needed? It's only used by the Source Tests, and from there,
//   only the `FacilityId` and `Name` properties are used.
public record FacilitySummary : IFacilityAirsName
{
    private FacilitySummary() { }
    public FacilitySummary(string id) => Id = (FacilityId)id;

    public FacilitySummary(Facility facility)
    {
        Id = facility.Id;
        Name = facility.Name;
        Description = facility.Description;
        FacilityAddress = facility.FacilityAddress;
        County = facility.County;
    }

    [Key]
    [Display(Name = "AIRS Number")]
    public FacilityId Id { get; } = null!;

    public string FacilityId => Id.FormattedId;

    [Display(Name = "Facility name")]
    public string Name { get; init; } = "";

    [Display(Name = "Facility description")]
    public string Description { get; init; } = "";

    // Location

    [Display(Name = "Company address")]
    public Address? FacilityAddress { get; init; }

    [Display(Name = "County")]
    public string County { get; init; } = "";
}
