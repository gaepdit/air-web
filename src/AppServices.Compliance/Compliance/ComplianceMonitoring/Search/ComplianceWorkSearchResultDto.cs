using AirWeb.AppServices.Compliance.FacilitySearch;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

public record ComplianceWorkSearchResultDto : IFacilitySearchResult
{
    public int Id { get; init; }
    public required string ComplianceWorkType { get; init; }
    public required string FacilityId { get; init; }
    public string? FacilityName { get; set; }
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public required string EventDateName { get; init; }
    public DateOnly? AdditionalDate { get; init; }
    public string? AdditionalDateName { get; init; }
    public bool IsReportable { get; init; }
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
