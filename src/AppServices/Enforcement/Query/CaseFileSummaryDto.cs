using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.Cases;

namespace AirWeb.AppServices.Enforcement.Query;

public record CaseFileSummaryDto : IIsClosedAndIsDeleted
{
    public int Id { get; init; }
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }

    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    [Display(Name = "Status")]
    public CaseFileStatus CaseFileStatus { get; init; }

    [Display(Name = "Discovery Date")]
    public DateOnly? DiscoveryDate { get; init; }

    public string Notes { get; init; } = null!;
}
