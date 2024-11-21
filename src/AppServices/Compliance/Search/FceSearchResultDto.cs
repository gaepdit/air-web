using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Search;

public record FceSearchResultDto : IStandardSearchResult
{
    public int Id { get; init; }
    public required string FacilityId { get; init; }
    public string? FacilityName { get; set; }
    public int Year { get; init; }
    public StaffViewDto? ReviewedBy { get; init; }
    public DateOnly CompletedDate { get; init; }
    public bool OnsiteInspection { get; init; }
    public bool IsDeleted { get; init; }
}
