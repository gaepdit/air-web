using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.Query;

public record CaseFileViewDto : ICloseableAndDeletable, IHasOwnerAndDeletable
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
        CaseFileStatus.CaseOpen => "bg-warning-subtle",
        CaseFileStatus.CaseClosed => "bg-info-subtle",
        _ => "bg-success-subtle",
    };

    [Display(Name = "Violation Type")]
    public ViolationType? ViolationType { get; init; }

    [Display(Name = "Discovery Date")]
    public DateOnly? DiscoveryDate { get; init; }

    [Display(Name = "Day Zero")]
    public DateOnly? DayZero { get; init; }

    public bool HasFormalEnforcement => EnforcementActions.Exists(action =>
        action is { IsDeleted: false, IsIssued: true } &&
        EnforcementAction.IsFormalEnforcementAction(action.ActionType));

    public string Notes { get; init; } = null!;
    public ICollection<Pollutant> Pollutants { get; } = [];

    [Display(Name = "Air Programs")]
    public ICollection<AirProgram> AirPrograms { get; } = [];

    public IList<WorkEntrySearchResultDto> ComplianceEvents { get; } = [];

    [UsedImplicitly]
    public List<CommentViewDto> Comments { get; } = [];

    public List<EnforcementActionViewDto> EnforcementActions { get; } = [];

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
