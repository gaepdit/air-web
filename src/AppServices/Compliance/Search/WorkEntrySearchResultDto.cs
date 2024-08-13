using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchResultDto : IStandardSearchResult
{
    public int Id { get; init; }
    public WorkEntryType WorkEntryType { get; [UsedImplicitly] init; } = default!;
    public string FacilityId { get; init; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public string EventDateName { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
