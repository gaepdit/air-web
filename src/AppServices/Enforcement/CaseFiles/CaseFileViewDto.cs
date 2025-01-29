using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.EnforcementActions;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.CaseFiles;

public record CaseFileViewDto : IIsClosedAndIsDeleted, IHasOwnerAndDeletable
{
    public int Id { get; init; }
    public bool IsClosed { get; init; }
    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    public CaseFileStatus CaseFileStatus { get; init; }

    public string CaseStatusClass => CaseFileStatus switch
    {
        CaseFileStatus.Open => "bg-warning-subtle",
        CaseFileStatus.Draft => "bg-info-subtle",
        CaseFileStatus.SubjectToComplianceSchedule => "bg-success-subtle",
        CaseFileStatus.Closed => "bg-primary-subtle",
        _ => "",
    };

    [Display(Name = "Violation Type")]
    public ViolationType? ViolationType { get; init; }

    [Display(Name = "Discovery Date")]
    public DateOnly? DiscoveryDate { get; init; }

    [Display(Name = "Day Zero")]
    public DateOnly? DayZero { get; init; }

    public bool HasFormalEnforcement => EnforcementActions.Exists(action =>
        action is { IsDeleted: false, Status: EnforcementActionStatus.Issued } &&
        EnforcementAction.IsFormalEnforcementAction(action.ActionType));

    public string Notes { get; init; } = null!;
    public IList<Pollutant> Pollutants { get; } = [];

    [Display(Name = "Air Programs")]
    public IList<AirProgram> AirPrograms { get; } = [];

    public IList<WorkEntrySearchResultDto> ComplianceEvents { get; } = [];

    [UsedImplicitly]
    public List<CommentViewDto> Comments { get; } = [];

    public List<IActionViewDto> EnforcementActions { get; } = [];

    // Properties: Closure
    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted by")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    // Calculated properties
    public string OwnerId => ResponsibleStaff?.Id ?? string.Empty;
}
