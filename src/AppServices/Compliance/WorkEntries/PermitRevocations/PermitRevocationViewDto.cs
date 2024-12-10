using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public record PermitRevocationViewDto : WorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Permit Revocation Date")]
    public DateOnly PermitRevocationDate { get; init; }

    [Display(Name = "Physical Shutdown Date")]
    public DateOnly? PhysicalShutdownDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
