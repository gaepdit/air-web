using IaipDataService.Structs;
using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Facilities;

public record FacilitySummary : IFacilityIdName
{
    public FacilitySummary() { }

    public FacilitySummary(Facility facility)
    {
        Id = facility.Id;
        Name = facility.Name;
        City = facility.FacilityAddress?.City ?? string.Empty;
        State = facility.FacilityAddress?.State ?? string.Empty;
        GeoCoordinates = facility.GeoCoordinates;
    }

    [Key]
    public FacilityId Id { get; init; } = null!;

    public string Name { get; init; } = null!;
    public GeoCoordinates? GeoCoordinates { get; set; }

    private string City { get; init; } = null!;
    private string State { get; init; } = null!;

    public string FacilityId => Id.FormattedId;
    public string Location => new[] { City, State }.ConcatWithSeparator(", ");
}
