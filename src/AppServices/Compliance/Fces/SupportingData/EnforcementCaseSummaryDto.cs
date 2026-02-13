using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record EnforcementCaseSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    [Display(Name = "Initial Action Date")]
    public DateOnly EnforcementDate { get; init; }

    [Display(Name = "Status")]
    public CaseFileStatus CaseFileStatus { get; init; }

    [Display(Name = "Latest Action")]
    public EnforcementActionType? LatestActionActionType { get; init; }

    [Display(Name = "Latest Action Date")]
    public DateOnly? LatestActionIssueDate { get; init; }
}
