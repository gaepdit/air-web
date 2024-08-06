using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.Compliance.Search;

public record FceSearchResultDto : IStandardSearchResult
{
    public int Id { get; init; }
    public string FacilityId { get; init; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public int Year { get; init; }
    public ApplicationUser? ReviewedBy { get; init; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; init; }
    public bool IsDeleted { get; init; }
}
