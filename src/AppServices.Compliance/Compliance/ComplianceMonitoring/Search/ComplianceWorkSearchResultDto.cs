using AirWeb.AppServices.Compliance.FacilitySearch;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

public record ComplianceWorkSearchResultDto : IFacilitySearchResult
{
    public int Id { get; init; }
    public ComplianceWorkType ComplianceWorkType { get; [UsedImplicitly] init; }
    public required string FacilityId { get; init; }
    public string? FacilityName { get; set; }
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public required string EventDateName { get; init; }
    public bool IsReportable { get; init; }
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
