using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.AppServices.FacilitySearch;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.AppServices.Enforcement.Search;

public record CaseFileSearchResultDto : IFacilitySearchResult
{
    public int Id { get; init; }
    public required string FacilityId { get; init; }
    public string? FacilityName { get; set; }
    public CaseFileStatus CaseFileStatus { get; init; }
    public DateOnly? DiscoveryDate { get; init; }
    public DateOnly? DayZero { get; init; }
    public StaffViewDto? ResponsibleStaff { get; init; }
    public bool IsDeleted { get; init; }
}
