using IaipDataService.Structs;
using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    public string Id { get; init; } = null!;

    public string Name { get; init; } = null!;
    public GeoCoordinates? GeoCoordinates { get; set; }

    private string City { get; init; } = null!;
    private string State { get; init; } = null!;

    // The `Id` property is needed to match existing DB procs. 
    // The `FacilityId` property is needed to satisfy the `IFacilityIdName` interface.
    [JsonIgnore]
    public string FacilityId => Id;

    public string Location => new[] { City, State }.ConcatWithSeparator(", ");
}
