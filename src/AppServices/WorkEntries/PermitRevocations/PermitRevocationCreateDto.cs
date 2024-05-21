using AirWeb.AppServices.WorkEntries.BaseCommandDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.PermitRevocations;

public record PermitRevocationCreateDto : BaseWorkEntryCreateDto
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
