using IaipDataService.Structs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.Facilities;

public record Facility : IFacilityIdName
{
    [UsedImplicitly]
    private Facility() { } // Used by ORM.

    public Facility(string id) => Id = (FacilityId)id;

    [Key]
    [JsonIgnore]
    [Display(Name = "AIRS Number")]
    public FacilityId Id { get; init; } = null!;

    public string FacilityId => Id.FormattedId;

    // Description

    [Display(Name = "Facility name")]
    public string Name { get; init; } = null!;

    [Display(Name = "Description")]
    public string Description { get; init; } = null!;

    // Location

    [Display(Name = "Company address")]
    public Address? FacilityAddress { get; set; }

    [Display(Name = "County")]
    public string County { get; init; } = "";

    [Display(Name = "Geographic coordinates")]
    public GeoCoordinates? GeoCoordinates { get; set; }

    // Regulatory data

    public RegulatoryData? RegulatoryData { get; set; }
}
