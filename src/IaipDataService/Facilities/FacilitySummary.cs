using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Facilities;

public record FacilitySummary : IFacilityAirsName
{
    private FacilitySummary() { }
    public FacilitySummary(string id) => Id = (FacilityId)id;

    public FacilitySummary(Facility facility)
    {
        Id = facility.Id;
        Name = facility.Name;
        City = facility.FacilityAddress?.City;
        State = facility.FacilityAddress?.State;
        County = facility.County;
    }

    [Key]
    [Display(Name = "AIRS Number")]
    public FacilityId Id { get; } = null!;

    public string FacilityId => Id.FormattedId;

    [Display(Name = "Facility name")]
    public string Name { get; init; } = "";

    public string County { get; init; } = "";
    public string? City { get; [UsedImplicitly] init; }
    public string? State { get; [UsedImplicitly] init; }
}
