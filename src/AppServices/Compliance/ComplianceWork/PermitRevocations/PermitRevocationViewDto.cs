using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Query;

namespace AirWeb.AppServices.Compliance.ComplianceWork.PermitRevocations;

public record PermitRevocationViewDto : WorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Permit Revocation Date")]
    public DateOnly? PermitRevocationDate { get; init; }

    [Display(Name = "Physical Shutdown Date")]
    public DateOnly? PhysicalShutdownDate { get; init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; init; }
}
