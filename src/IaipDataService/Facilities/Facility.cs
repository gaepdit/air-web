using IaipDataService.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IaipDataService.Facilities;

public record Facility
{
    public Facility() { }
    public Facility(string id) => Id = (FacilityId)id;

    [Key]
    [JsonIgnore]
    [Display(Name = "AIRS Number")]
    public FacilityId Id { get; init; } = null!;

    public string FacilityId
    {
        get => Id.FormattedId;
        init
        {
            if (IaipDataService.Facilities.FacilityId.IsValidFormat(value))
                Id = (FacilityId)value;
        }
    }

    // Description

    [Display(Name = "Facility name")]
    public string Name { get; init; } = "";

    [Display(Name = "Facility description")]
    public string Description { get; init; } = "";

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
