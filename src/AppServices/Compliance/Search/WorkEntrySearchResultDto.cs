using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchResultDto : IStandardSearchResult
{
    public int Id { get; init; }
    public string WorkType { get; init; } = string.Empty;
    public string FacilityId { get; init; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public string EventDateName { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }

    public string WorkTypeDisplay => WorkType switch
    {
        "AnnualComplianceCertification" => "Annual Compliance Certification",
        "Inspection" => "Inspection",
        "RmpInspection" => "RMP Inspection",
        "Report" => "Reports",
        "SourceTestReview" => "Source Test Review",
        "Notification" => "Notification",
        "PermitRevocation" => "Permit Revocation",
        _ => "Unknown",
    };
}
